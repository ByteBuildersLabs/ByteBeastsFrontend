using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageNumber : MonoBehaviour {

    // UI Text component that will display the damage amount
    public Text damageText;

    // How long the damage number will stay on screen before disappearing
    public float lifetime = 1f;

    // Speed at which the damage number will move upwards
    public float moveSpeed = 1f;

    // Random jitter applied to the position to make the damage number appear less static
    public float placementJitter = .5f;

    // Use this for initialization
    void Start () {
        // Initialization can be done here if needed
    }
    
    // Update is called once per frame
    void Update () {
        // Destroy the damage number object after its lifetime has expired
        Destroy(gameObject, lifetime);

        // Move the damage number upwards based on the moveSpeed and time elapsed
        transform.position += new Vector3(0f, moveSpeed * Time.deltaTime, 0f);
    }

    // Method to set the damage amount and apply jitter to the position
    public void SetDamage(int damageAmount)
    {
        // Set the damage amount text
        damageText.text = damageAmount.ToString();

        // Apply random jitter to the position for visual effect
        transform.position += new Vector3(Random.Range(-placementJitter, placementJitter), Random.Range(-placementJitter, placementJitter), 0f);
    }
}
