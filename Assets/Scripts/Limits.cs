using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Limits : MonoBehaviour
{   
    private Transform transform;
    public Vector2 HorizontalRange = Vector2.zero;
    public Vector2 VerticalRange = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate()
    {
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, VerticalRange.x, VerticalRange.y),
            Mathf.Clamp(transform.position.y, HorizontalRange.x, HorizontalRange.y),
            transform.position.z
        );
    }
}
