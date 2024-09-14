using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : WaypointMovement
{   
    // Movement direction of the NPC (Horizontal or Vertical), configurable from the Inspector.
    [SerializeField] private MovementDirection direction;

    private readonly int walkDown = Animator.StringToHash("WalkDown");

    // Start is called before the first frame update
    protected override void RotateCharacterAnimationHorizontal()
    {
        // Only rotate the character if the movement direction is horizontal.
        if (direction != MovementDirection.Horizontal) return;

        // If the next point is to the right of the last position, face right.
        if (NextPoint.x > lastPosition.x) 
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {   
            // If the next point is to the left of the last position, face left.
            transform.localScale = new Vector3(-1, 1, 1);
        }
    } 

    protected override void RotateCharacterAnimationVertical()
    {   
        // Only rotate the character if the movement direction is vertical.
        if (direction != MovementDirection.Vertical) return;

        // If the next point is above the last position, face up.
        if (NextPoint.y > lastPosition.y)
        {
            _animator.SetBool(walkDown, false);
        }
        else
        {
            _animator.SetBool(walkDown, true);
        }
    }
}
