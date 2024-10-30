using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages UI fading effects, primarily for transitioning between scenes or states.
/// </summary>
public class UIFade : MonoBehaviour {

	/// <value>
    /// Singleton instance of the UIFade manager.
    /// </value>
    public static UIFade instance;

	/// <value>
    /// Reference to the UI image used for fading.
    /// </value>
    public Image fadeScreen;

	/// <value>
    /// Speed of the fade effect.
    /// </value>
    public float fadeSpeed;

	/// <value>
    /// Flag indicating whether to fade to black.
    /// </value>
    public bool shouldFadeToBlack;

	/// <value>
    /// Flag indicating whether to fade from black.
    /// </value>
    public bool shouldFadeFromBlack;

	// Use this for initialization

	/// <summary>
    /// Called when the script is instantiated.
    /// Initializes the singleton instance and ensures the game object persists across scene loads.
    /// </summary>
	void Start () {
        instance = this;

        DontDestroyOnLoad(gameObject);

	}
	
	// Update is called once per frame

	/// <summary>
    /// Called every frame after Start().
    /// Handles the fading animation based on the flags set by FadeToBlack() and FadeFromBlack().
    /// </summary>
	void Update () {

        if (shouldFadeToBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 1f, fadeSpeed * Time.deltaTime));

            if(fadeScreen.color.a == 1f)
            {
                shouldFadeToBlack = false;
            }
        }

        if (shouldFadeFromBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 0f, fadeSpeed * Time.deltaTime));

            if (fadeScreen.color.a == 0f)
            {
                shouldFadeFromBlack = false;
            }
        }
    }
    /// <summary>
    /// Initiates a fade to black transition.
    /// </summary>
    public void FadeToBlack()
    {
        shouldFadeToBlack = true;
        shouldFadeFromBlack = false;

    }

	/// <summary>
    /// Initiates a fade from black transition.
    /// </summary>
    public void FadeFromBlack()
    {
        shouldFadeToBlack = false;
        shouldFadeFromBlack = true;
    }
}
