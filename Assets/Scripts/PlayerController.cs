using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Reference to the Rigidbody2D component for movement
    public Rigidbody2D theRB;
    
    // Speed at which the player moves
    public float moveSpeed;
    
    // Reference to the Animator component for animations
    public Animator myAnim;
    
    // Singleton instance of the PlayerController
    public static PlayerController instance;
    
    // Name of the area to transition to (not used in this script)
    public string areaTransitionName;
    
    // Bounds for player movement
    private Vector3 bottomLeftLimit;
    private Vector3 topRightLimit;
    
    // Reference to the joystick control (for mobile platforms)
    public FixedJoystick joystick;
    
    // Boolean to control whether the player can move
    public bool canMove = true;
    
    // Boolean to check if the player is currently moving (not used in this script)
    private bool isMoving = false;

    private void Awake()
    {
        // Ensure only one instance of PlayerController exists
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        // Prevent the PlayerController from being destroyed when loading new scenes
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // Initialize joystick if needed
        // joystick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<FixedJoystick>();
        // #if UNITY_STANDALONE_WIN
        //     joystick.gameObject.SetActive(false);
        // #endif
    }

    void Update()
    {
        // Read input from keyboard or joystick
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        
        //#if UNITY_ANDROID || UNITY_IOS || UNITY_WEBGL
        //    horizontal = joystick.Horizontal;
        //    vertical = joystick.Vertical;
        //#endif

        // Handle player movement
        if (canMove)
        {
            theRB.velocity = new Vector2(horizontal, vertical) * moveSpeed;
        }
        else
        {
            theRB.velocity = Vector2.zero;
        }

        // Update animation parameters based on movement
        myAnim.SetFloat("moveX", theRB.velocity.x);
        myAnim.SetFloat("moveY", theRB.velocity.y);

        // Update last movement direction
        if (horizontal == 1 || horizontal == -1 || vertical == 1 || vertical == -1)
        {
            if (canMove)
            {
                myAnim.SetFloat("lastMoveX", horizontal);
                myAnim.SetFloat("lastMoveY", vertical);
            }
        }

        // Clamp player position to prevent going out of bounds
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, bottomLeftLimit.x, topRightLimit.x),
            Mathf.Clamp(transform.position.y, bottomLeftLimit.y, topRightLimit.y),
            transform.position.z
        );
    }

    // Set the boundaries for player movement
    public void SetBounds(Vector3 botLeft, Vector3 topRight)
    {
        bottomLeftLimit = botLeft + new Vector3(.5f, 1f, 0f); // Adjust for player size
        topRightLimit = topRight + new Vector3(-.5f, -1f, 0f); // Adjust for player size
    }

    // Activate or deactivate the joystick based on the platform and value
    public void ActivateJoystick(bool val)
    {
        #if UNITY_ANDROID || UNITY_IOS || UNITY_WEBGL
            joystick.gameObject.SetActive(val);
        #endif
    }
}
