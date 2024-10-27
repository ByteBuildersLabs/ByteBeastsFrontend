using UnityEngine;

public class LeafMovement : MonoBehaviour { 

    public float swayAmount = 0.11f; 
    public float swaySpeed = 0.98f;   
    public float noiseScale = 0.2f;

    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        
        float swayX = Mathf.Sin(Time.time * swaySpeed) * swayAmount; 
        float swayY = Mathf.PerlinNoise(Time.time * noiseScale, 0) * swayAmount; 

        transform.localPosition = originalPosition + new Vector3(-swayX, swayY, 0); 
    }
}