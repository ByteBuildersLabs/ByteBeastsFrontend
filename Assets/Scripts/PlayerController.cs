using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using System;

public class PlayerController : MonoBehaviour
{

    /// <value>
    /// Reference to the Rigidbody2D component attached to the player object.
    /// </value>
    public Rigidbody2D theRB;

    /// <value>
    /// Speed at which the player moves horizontally and vertically.
    /// </value>
    public float moveSpeed;

    /// <value>
    /// Reference to the Animator component controlling the player's animations.
    /// </value>
    public Animator myAnim;

    /// <summary>
    /// Singleton instance of the PlayerController.
    /// </value>
    public static PlayerController instance;

    /// <value>
    /// Name of the area transition scene.
    /// </value>
    public string areaTransitionName;

    /// <value>
    /// Bottom-left limit of the playable area.
    /// </value>
    private Vector3 bottomLeftLimit;

    /// <value>
    /// Top-right limit of the playable area.
    /// </value>
    private Vector3 topRightLimit;

    /// <value>
    /// Reference to the FixedJoystick component for mobile input.
    /// </value>
    public FixedJoystick joystick;

    /// <value>
    /// Flag indicating whether the player can move.
    /// </value>
    public bool canMove = true;

    /// <value>
    /// Flag indicating whether the player is currently moving.
    /// </value>
    private bool isMoving = false;

    /// <summary>
    /// Stores the transform of the entrance of the last house entered, for retrieval when exiting a house. 
    /// </summary>
    public static Vector3 lastHouseEntered = Vector3.zero;

    /// <summary>
    /// Called when the script is instantiated.
    /// Initializes the singleton instance and ensures the GameObject isn't destroyed when loading new scenes.
    /// </summary>
    private void Awake()
    {
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

        DontDestroyOnLoad(gameObject);

    }

    /// <summary>
    /// Called when the script starts execution.
    /// Initializes the FixedJoystick component based on the platform.
    /// </summary>
    void Start()
    {
        joystick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<FixedJoystick>();

#if UNITY_STANDALONE_WIN
             joystick.gameObject.SetActive(false);
#endif

    }


    /// <summary>
    /// Called every frame after Start().
    /// Handles player movement and animation based on input.
    /// </summary>
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
#if UNITY_ANDROID || UNITY_IOS || UNITY_WEBGL
        horizontal = joystick.Horizontal;
        vertical = joystick.Vertical;
#endif

        if (canMove)
        {
            theRB.velocity = new Vector2(horizontal, vertical) * moveSpeed;
        }
        else
        {
            theRB.velocity = Vector2.zero;
        }

        myAnim.SetFloat("moveX", theRB.velocity.x);
        myAnim.SetFloat("moveY", theRB.velocity.y);

        if (horizontal == 1 || horizontal == -1 || vertical == 1 || vertical == -1)
        {
            if (canMove)
            {
                myAnim.SetFloat("lastMoveX", horizontal);
                myAnim.SetFloat("lastMoveY", vertical);
            }
        }

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, bottomLeftLimit.x, topRightLimit.x), Mathf.Clamp(transform.position.y, bottomLeftLimit.y, topRightLimit.y), transform.position.z);
    }

    /// <summary>
    /// Sets the bounds of the playable area.
    /// </summary>
    /// <param name="botLeft">Bottom-left corner of the area.</param>
    /// <param name="topRight">Top-right corner of the area.</param>
    public void SetBounds(Vector3 botLeft, Vector3 topRight)
    {
        bottomLeftLimit = botLeft + new Vector3(.5f, 1f, 0f);
        topRightLimit = topRight + new Vector3(-.5f, -1f, 0f);
    }

    /// <summary>
    /// Activates or deactivates the FixedJoystick component based on the platform.
    /// </summary>
    /// <param name="val">True to activate, false to deactivate.</param>
    public void ActivateJoystick(bool val)
    {
#if UNITY_ANDROID || UNITY_IOS || UNITY_WEBGL
        joystick.gameObject.SetActive(val);
#endif
    }
}
