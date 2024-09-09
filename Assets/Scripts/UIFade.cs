using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFade : MonoBehaviour {

    // Singleton instance of UIFade
    public static UIFade instance;

    // Image component used for fading effect
    public Image fadeScreen;

    // Speed of the fade effect
    public float fadeSpeed;

    // Flags to control fade direction
    public bool shouldFadeToBlack;
    public bool shouldFadeFromBlack;

    // Use this for initialization
    void Start () {
        // Set the singleton instance
        instance = this;

        // Ensure this object persists between scene loads
        DontDestroyOnLoad(gameObject);
    }
    
    // Update is called once per frame
    void Update () {
        // Fade to black if the flag is set
        if (shouldFadeToBlack)
        {
            // Gradually increase the alpha of the fadeScreen image to 1 (opaque)
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 1f, fadeSpeed * Time.deltaTime));

            // Check if the fade is complete
            if(fadeScreen.color.a == 1f)
            {
                shouldFadeToBlack = false;
            }
        }

        // Fade from black if the flag is set
        if (shouldFadeFromBlack)
        {
            // Gradually decrease the alpha of the fadeScreen image to 0 (transparent)
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 0f, fadeSpeed * Time.deltaTime));

            // Check if the fade is complete
            if (fadeScreen.color.a == 0f)
            {
                shouldFadeFromBlack = false;
            }
        }
    }

    // Start fading to black
    public void FadeToBlack()
    {
        shouldFadeToBlack = true;
        shouldFadeFromBlack = false;
    }

    // Start fading from black
    public void FadeFromBlack()
    {
        shouldFadeToBlack = false;
        shouldFadeFromBlack = true;
    }
}
