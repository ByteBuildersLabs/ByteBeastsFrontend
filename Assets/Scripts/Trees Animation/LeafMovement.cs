using UnityEngine;

public class LeafMovement : MonoBehaviour
{
    public float swayAmount = 0.11f;      
    public float swaySpeed = 0.98f;       
    public float noiseScale = 0.47f;      
    public float rotationAmount = 2f;     
    public float rotationSpeed = 0.3f;    

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private float timeOffset;             

    void Start()
    {
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;

        timeOffset = Random.Range(0f, 10f); 
    }

    void Update()
    {
   
        float swayX = Mathf.Sin((Time.time + timeOffset) * swaySpeed) * swayAmount;
        float swayY = Mathf.PerlinNoise((Time.time + timeOffset) * noiseScale, 0) * swayAmount;

        
        transform.localPosition = originalPosition + new Vector3(swayX, swayY, 0);

   
        float rotationZ = Mathf.Sin((Time.time + timeOffset) * rotationSpeed) * rotationAmount;
        transform.localRotation = originalRotation * Quaternion.Euler(0, 0, rotationZ);
    }
}
