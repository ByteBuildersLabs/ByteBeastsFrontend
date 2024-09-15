using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script makes the Canvas follow the NPC without inheriting its rotation.
public class FollowNPC : MonoBehaviour
{
    [SerializeField] private Transform npcTransform; // Reference to the NPC's Transform

    private Vector3 offset; // Offset between the NPC and the Canvas

    void Start()
    {
        // Calculate the initial offset between the NPC and the Canvas.
        offset = transform.position - npcTransform.position;
    }

    void LateUpdate()
    {
        // Update the Canvas position to follow the NPC, maintaining the offset.
        transform.position = npcTransform.position + offset;

        // Reset the Canvas rotation to always stay upright.
        transform.rotation = Quaternion.Euler(0, 0, 0); // Keeps the rotation fixed.
    }
}