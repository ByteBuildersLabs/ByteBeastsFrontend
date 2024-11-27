using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Manages the camera behavior in the game, including following the player and maintaining bounds.
/// </summary>
public class CameraController : MonoBehaviour
{

    // Target that the camera will follow
    public Transform target;

    // Tilemap that defines the bounds for the camera
    public Tilemap theMap;

    // Variables to hold the limits of the camera's movement
    private Vector3 bottomLeftLimit;
    private Vector3 topRightLimit;

    // Variables to calculate camera bounds based on its size
    private float halfHeight;
    private float halfWidth;

    // Index of the background music to play
    public int musicToPlay;
    private bool musicStarted;

    /// <summary>
    /// Initializes the CameraController component.
    /// Sets up the camera target, calculates its bounds, and configures the player's movement limits.
    /// </summary>
    void Start()
    {
        // Set the target to be the player object
        target = PlayerController.instance.transform;
        // target = FindObjectOfType<PlayerController>().transform;

        // Calculate the half-height and half-width of the camera's view
        halfHeight = Camera.main.orthographicSize;
        halfWidth = halfHeight * Camera.main.aspect;

        // Get the bounds of the Tilemap and adjust them for camera size
        theMap.CompressBounds();
        bottomLeftLimit = theMap.localBounds.min + new Vector3(halfWidth, halfHeight, 0f);
        topRightLimit = theMap.localBounds.max + new Vector3(-halfWidth, -halfHeight, 0f);

        // Set the bounds for the player to keep them within the map limits
        PlayerController.instance.SetBounds(theMap.localBounds.min, theMap.localBounds.max);
    }

	/// <summary>
    /// Called once per frame after Update.
    /// Updates the camera's position to follow the target and ensures it stays within the defined bounds.
    /// Also plays background music if it hasn't been started yet.
    /// </summary>
    void LateUpdate()
    {
        // Update the camera's position to follow the target
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);

        // Clamp the camera's position to stay within the defined bounds
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, bottomLeftLimit.x, topRightLimit.x), Mathf.Clamp(transform.position.y, bottomLeftLimit.y, topRightLimit.y), transform.position.z);

        // Play background music if it hasn't been started yet
        if (!musicStarted)
        {
            musicStarted = true;
            AudioManager.instance.PlayBGM(musicToPlay);
        }
    }
}