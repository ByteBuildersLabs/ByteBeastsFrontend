﻿//
// Rain Maker (c) 2015 Digital Ruby, LLC
// http://www.digitalruby.com
//

using UnityEngine;
using System.Collections;
using static UnityEngine.ParticleSystem;
using UnityEngine.SceneManagement;

namespace DigitalRuby.RainMaker
{
    public class RainScript2D : BaseRainScript
    {
        private static readonly Color32 explosionColor = new Color32(255, 255, 255, 255);
        private static RainScript2D instance; // Singleton instance
        private float cameraMultiplier = 1.0f;
        private Bounds visibleBounds = new Bounds();
        private float yOffset;
        private float visibleWorldWidth;
        private float initialEmissionRain;
        private Vector2 initialStartSpeedRain;
        private Vector2 initialStartSizeRain;
        private Vector2 initialStartSpeedMist;
        private Vector2 initialStartSizeMist;
        private Vector2 initialStartSpeedExplosion;
        private Vector2 initialStartSizeExplosion;
        private readonly ParticleSystem.Particle[] particles = new ParticleSystem.Particle[2048];
        private Camera mainCamera;
        private AudioSource audioSource;
        private bool isRainingInCurrentScene = false;

        [Tooltip("The starting y offset for rain and mist. This will be offset as a percentage of visible height from the top of the visible world.")]
        public float RainHeightMultiplier = 0.15f;

        [Tooltip("The total width of the rain and mist as a percentage of visible width")]
        public float RainWidthMultiplier = 1.5f;

        [Tooltip("Collision mask for the rain particles")]
        public LayerMask CollisionMask = -1;

        [Tooltip("Lifetime to assign to rain particles that have collided. 0 for instant death. This can allow the rain to penetrate a little bit beyond the collision point.")]
        [Range(0.0f, 0.5f)]
        public float CollisionLifeTimeRain = 0.02f;

        [Tooltip("Multiply the velocity of any mist colliding by this amount")]
        [Range(0.0f, 0.99f)]
        public float RainMistCollisionMultiplier = 0.75f;

        private void EmitExplosion(ref Vector3 pos)
        {
            int count = UnityEngine.Random.Range(2, 5);
            while (count != 0)
            {
                float xVelocity = UnityEngine.Random.Range(-2.0f, 2.0f) * cameraMultiplier;
                float yVelocity = UnityEngine.Random.Range(1.0f, 3.0f) * cameraMultiplier;
                float lifetime = UnityEngine.Random.Range(0.1f, 0.2f);
                float size = UnityEngine.Random.Range(0.05f, 0.1f) * cameraMultiplier;
                ParticleSystem.EmitParams param = new ParticleSystem.EmitParams();
                param.position = pos;
                param.velocity = new Vector3(xVelocity, yVelocity, 0.0f);
                param.startLifetime = lifetime;
                param.startSize = size;
                param.startColor = explosionColor;
                RainExplosionParticleSystem.Emit(param, 1);
                count--;
            }
        }

        private void Awake()
        {
            // Only assign the Singleton if we are in the scene that requires rain.
            if (instance == null && SceneManager.GetActiveScene().name.Contains("DarkForest"))
            {
                instance = this;
                SceneManager.sceneLoaded += OnSceneLoaded;
            }
            else if (instance != this)
            {
                Destroy(gameObject); // Update the camera reference in each scene
            }
        }
        private void OnDestroy()
        {
            if (instance == this)
            {
                SceneManager.sceneLoaded -= OnSceneLoaded;
            }
        }
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // Update camera reference in each scene
            mainCamera = Camera.main;

            // Activate or deactivate the rain according to the scene
            if (scene.name.Contains("DarkForest"))
            {
                EnableRain();
                isRainingInCurrentScene = true;
            }
            else
            {
                DisableRain();
                isRainingInCurrentScene = false;
                Destroy(gameObject); // Destroy the object if we leave the rain scene.
            }
        }


        private void EnableRain()
        {
            // Ensure that the particle system and rain sound are active.
            if (!RainFallParticleSystem.isPlaying)
            {
                RainFallParticleSystem.Clear();
                RainFallParticleSystem.Play();
            }

            if (audioSource != null && !audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }

        private void DisableRain()
        {
            // Stop the particle system and the rain sound
            if (RainFallParticleSystem.isPlaying)
            {
                RainFallParticleSystem.Stop();
            }

            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }

        private void LateUpdate()
        {
            // Check that the camera reference is updated
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
                if (mainCamera == null)
                {
                    Debug.LogWarning("Main Camera not found. RainScript2D will wait until a camera is available.");
                    return; // Salir si no se encuentra la cámara, evitará que el resto del código se ejecute
                }
            }

            // Controlling the positioning of the particles to follow the camera
            if (FollowCamera && mainCamera != null)
            {
                transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, transform.position.z);
            }
        }

        private void OnDisable()
        {
            // Make sure to stop the rain when the script is deactivated (e.g., when changing scenes).
            if (!isRainingInCurrentScene)
            {
                DisableRain();
            }
        }






        private void TransformParticleSystem(ParticleSystem p, Vector2 initialStartSpeed, Vector2 initialStartSize)
        {
            if (p == null)
            {
                return;
            }
            if (FollowCamera)
            {
                p.transform.position = new Vector3(Camera.transform.position.x, visibleBounds.max.y + yOffset, p.transform.position.z);
            }
            else
            {
                p.transform.position = new Vector3(p.transform.position.x, visibleBounds.max.y + yOffset, p.transform.position.z);
            }
            p.transform.localScale = new Vector3(visibleWorldWidth * RainWidthMultiplier, 1.0f, 1.0f);
            var m = p.main;
            var speed = m.startSpeed;
            var size = m.startSize;
            speed.constantMin = initialStartSpeed.x * cameraMultiplier;
            speed.constantMax = initialStartSpeed.y * cameraMultiplier;
            size.constantMin = initialStartSize.x * cameraMultiplier;
            size.constantMax = initialStartSize.y * cameraMultiplier;
            m.startSpeed = speed;
            m.startSize = size;

           
        }

        private void CheckForCollisionsRainParticles()
        {
            int count = 0;
            bool changes = false;

            if (CollisionMask != 0)
            {
                count = RainFallParticleSystem.GetParticles(particles);
                RaycastHit2D hit;

                for (int i = 0; i < count; i++)
                {
                    Vector3 pos = particles[i].position + RainFallParticleSystem.transform.position;
                    hit = Physics2D.Raycast(pos, particles[i].velocity.normalized, particles[i].velocity.magnitude * Time.deltaTime);
                    if (hit.collider != null && ((1 << hit.collider.gameObject.layer) & CollisionMask) != 0)
                    {
                        if (CollisionLifeTimeRain == 0.0f)
                        {
                            particles[i].remainingLifetime = 0.0f;
                        }
                        else
                        {
                            particles[i].remainingLifetime = Mathf.Min(particles[i].remainingLifetime, UnityEngine.Random.Range(CollisionLifeTimeRain * 0.5f, CollisionLifeTimeRain * 2.0f));
                            pos += (particles[i].velocity * Time.deltaTime);
                        }
                        changes = true;
                    }
                }
            }

            if (RainExplosionParticleSystem != null)
            {
                if (count == 0)
                {
                    count = RainFallParticleSystem.GetParticles(particles);
                }
                for (int i = 0; i < count; i++)
                {
                    if (particles[i].remainingLifetime < 0.24f)
                    {
                        Vector3 pos = particles[i].position + RainFallParticleSystem.transform.position;
                        EmitExplosion(ref pos);
                    }
                }
            }
            if (changes)
            {
                RainFallParticleSystem.SetParticles(particles, count);
            }
        }

        private void CheckForCollisionsMistParticles()
        {
            if (RainMistParticleSystem == null || CollisionMask == 0)
            {
                return;
            }

            int count = RainMistParticleSystem.GetParticles(particles);
            bool changes = false;
            RaycastHit2D hit;

            for (int i = 0; i < count; i++)
            {
                Vector3 pos = particles[i].position + RainMistParticleSystem.transform.position;
                hit = Physics2D.Raycast(pos, particles[i].velocity.normalized, particles[i].velocity.magnitude* Time.deltaTime, CollisionMask);
                if (hit.collider != null)
                {
                    particles[i].velocity *= RainMistCollisionMultiplier;
                    changes = true;
                }
            }

            if (changes)
            {
                RainMistParticleSystem.SetParticles(particles, count);
            }
        }

        private IEnumerator StartRainAfterLoad()
        {
            yield return new WaitForSeconds(0.1f); // Expect a short delay to avoid glitches
            RainFallParticleSystem.Play();
        }

        protected override void Start()
        {
            base.Start();
            
            initialEmissionRain = RainFallParticleSystem.emission.rateOverTime.constant;
            initialStartSpeedRain = new Vector2(RainFallParticleSystem.main.startSpeed.constantMin, RainFallParticleSystem.main.startSpeed.constantMax);
            initialStartSizeRain = new Vector2(RainFallParticleSystem.main.startSize.constantMin, RainFallParticleSystem.main.startSize.constantMax);

            if (RainMistParticleSystem != null)
            {
                initialStartSpeedMist = new Vector2(RainMistParticleSystem.main.startSpeed.constantMin, RainMistParticleSystem.main.startSpeed.constantMax);
                initialStartSizeMist = new Vector2(RainMistParticleSystem.main.startSize.constantMin, RainMistParticleSystem.main.startSize.constantMax);
            }

            if (RainExplosionParticleSystem != null)
            {
                initialStartSpeedExplosion = new Vector2(RainExplosionParticleSystem.main.startSpeed.constantMin, RainExplosionParticleSystem.main.startSpeed.constantMax);
                initialStartSizeExplosion = new Vector2(RainExplosionParticleSystem.main.startSize.constantMin, RainExplosionParticleSystem.main.startSize.constantMax);
            }

            RainFallParticleSystem.Stop();
            StartCoroutine(StartRainAfterLoad());
            RainFallParticleSystem.Clear();
            RainFallParticleSystem.Play();


        }

        protected override void Update()
        {
            base.Update();

            cameraMultiplier = (Camera.orthographicSize * 0.25f);
            visibleBounds.min = Camera.main.ViewportToWorldPoint(Vector3.zero);
            visibleBounds.max = Camera.main.ViewportToWorldPoint(Vector3.one);
            visibleWorldWidth = visibleBounds.size.x;
            yOffset = (visibleBounds.max.y - visibleBounds.min.y) * RainHeightMultiplier;

            TransformParticleSystem(RainFallParticleSystem, initialStartSpeedRain, initialStartSizeRain);
            TransformParticleSystem(RainMistParticleSystem, initialStartSpeedMist, initialStartSizeMist);
            TransformParticleSystem(RainExplosionParticleSystem, initialStartSpeedExplosion, initialStartSizeExplosion);

            CheckForCollisionsRainParticles();
            CheckForCollisionsMistParticles();
        }

        protected override float RainFallEmissionRate()
        {
            return initialEmissionRain * RainIntensity;
        }

        protected override bool UseRainMistSoftParticles
        {
            get
            {
                return false;
            }
        }
    }
}