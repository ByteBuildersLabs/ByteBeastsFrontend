using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The Waypoint class is a Unity component that can be attached to GameObjects.
// It allows defining a series of points and visualizing them in the scene.
public class Waypoint : MonoBehaviour
{
    // An array of Vector3 points that define the waypoints relative to the object's position.
    // SerializedField allows it to be set in the Unity Inspector but keeps it private.
    [SerializeField] private Vector3[] points;

    // A public read-only property that exposes the points array to other scripts.
    public Vector3[] Points => points;

    // Stores the current position of the GameObject. It's private set, so it can only be modified inside this class.
    public Vector3 ActualPosition { get; private set; }

    // A boolean to track if the game has started.
    private bool isGameStarted;

    // Start is called before the first frame update.
    void Start()
    {   
        // Set the game state to started.
        isGameStarted = true;

        // Set the actual position to the GameObject's current transform position.
        ActualPosition = transform.position;
    }

    // OnDrawGizmos is called by Unity to draw Gizmos in the Scene view, useful for visual debugging.
    private void OnDrawGizmos()
    {   
        // If the game hasn't started and the transform position has changed, update the actual position.
        if (isGameStarted == false && transform.hasChanged) 
            ActualPosition = transform.position;

        // If there are no points defined or the array is empty, exit the method early.
        if (points == null || points.Length <= 0) 
            return;
        
        // Iterate through each point in the points array.
        for (int i = 0; i < points.Length; i++)
        {   
            // Set the Gizmos color to blue to draw the current point.
            Gizmos.color = Color.blue;

            // Draw a wireframe sphere at the current point position offset by the actual position.
            Gizmos.DrawWireSphere(points[i] + ActualPosition, 0.5f);

            // Check if the current point is not the last in the array to avoid index out of bounds error.
            if (i < points.Length - 1)
            {
                // Set the Gizmos color to gray to draw the connecting line between points.
                Gizmos.color = Color.gray;

                // Draw a line between the current point and the next point in the sequence.
                Gizmos.DrawLine(points[i] + ActualPosition, points[i + 1] + ActualPosition);
            }
        }
    }
}
