using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    // Array of AudioSources for sound effects (SFX)
    public AudioSource[] sfx;
    // Array of AudioSources for background music (BGM)
    public AudioSource[] bgm;

    // Singleton instance of AudioManager
    public static AudioManager instance;

    // Called when the script is first run or when the object is instantiated
    void Start () {
        // Set the singleton instance to this object
        instance = this;

        // Prevent this object from being destroyed when loading new scenes
        DontDestroyOnLoad(this.gameObject);
    }
    
    // Update is called once per frame (currently unused)
    void Update () {
    }

    // Play a sound effect based on the provided index
    public void PlaySFX(int soundToPlay)
    {
        // Check if the index is within the bounds of the sfx array
        if (soundToPlay < sfx.Length)
        {
            // Play the sound effect corresponding to the index
            sfx[soundToPlay].Play();
        }
    }

    // Play background music based on the provided index
    public void PlayBGM(int musicToPlay)
    {
        // Check if the specified music track is not already playing
        if (!bgm[musicToPlay].isPlaying)
        {
            // Stop any currently playing background music
            StopMusic();

            // Check if the index is within the bounds of the bgm array
            if (musicToPlay < bgm.Length)
            {
                // Play the background music corresponding to the index
                bgm[musicToPlay].Play();
            }
        }
    }

    // Stop all background music
    public void StopMusic()
    {
        // Iterate over all background music sources and stop them
        for(int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
    }
}
