using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enumeration defining movement directions that the character can follow.
public enum MovementDirection {
    Horizontal, // Movement is left or right.
    Vertical    // Movement is up or down.
}

// This class controls the movement of a GameObject along a series of waypoints.
public class WaypointMovement : MonoBehaviour
{   
    // Speed at which the character moves between waypoints, configurable from the Inspector.
    [SerializeField] protected float speed;

    // Property that calculates the next target point from the Waypoint component based on the current index.
    public Vector3 NextPoint => _waypoint.GetMovementPosition(currentPointIndex);

    // Reference to the Waypoint component attached to the same GameObject.
    protected Waypoint _waypoint;

    // Index of the current point the character is moving towards.
    protected int currentPointIndex;

    // Stores the last position of the character, used for determining movement direction for animations.
    protected Vector3 lastPosition;

    //Reference to the Animator component attached to the same GameObject.
    protected Animator _animator;

    // Start is called before the first frame update.
    void Start()
    {   
        // Initialize the current point index to start at the first waypoint.
        currentPointIndex = 0;

        // Get the Waypoint component attached to this GameObject.
        _waypoint = GetComponent<Waypoint>();

        // Get the Animator component attached to this GameObject.
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame.
    void Update()
    {
        // Moves the character towards the current target point.
        MoveCharacter();

        // Rotates the character's animation based on movement direction.
        RotateCharacterAnimationHorizontal();
        RotateCharacterAnimationVertical();

        // Checks if the character has reached the current target point.
        if (CheckCurrentPointReached())
        {
            // Updates the index to move to the next point once the current one is reached.
            UpdateMovementIndex();
        }
    }

    // Moves the character towards the next point using linear interpolation based on speed.
    private void MoveCharacter()
    {
        transform.position = Vector3.MoveTowards(transform.position, NextPoint, speed * Time.deltaTime);
    }

    // Checks if the character has reached the current point by comparing distances.
    private bool CheckCurrentPointReached()
    {
        // Calculate the distance between the character's current position and the target point.
        float distanceToCurrentPoint = (transform.position - NextPoint).magnitude;

        // If the character is close enough to the target point, mark the current point as reached.
        if (distanceToCurrentPoint < 0.1f)
        {   
            // Store the current position to help with direction determination in animations.
            lastPosition = transform.position;
            return true;
        }
        
        // If not close enough, the point is not yet reached.
        return false;
    }

    // Updates the index to determine which point the character should move towards next.
    private void UpdateMovementIndex()
    {
        // If the character is at the last point, reset the index to loop back to the first point.
        if (currentPointIndex == _waypoint.Points.Length - 1)
        {
            currentPointIndex = 0;
        }
        else
        {
            // Otherwise, increment the index to move to the next point.
            if (currentPointIndex < _waypoint.Points.Length - 1)
            {
                currentPointIndex++;
            }
        }
    }

    // Handles the rotation of the character based on the direction they are moving.
    protected virtual void RotateCharacterAnimationHorizontal()
    {
        
    }

    protected virtual void RotateCharacterAnimationVertical()
    {
        
    }
}
