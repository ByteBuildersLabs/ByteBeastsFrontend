using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script manages audio in the game, including both sound effects and background music.
/// </summary>
public class AudioManager : MonoBehaviour {

	/// <value>
    /// Array of AudioSource components for sound effects.
    /// Each AudioSource corresponds to a specific sound effect.
    /// </value>
    public AudioSource[] sfx;

	/// <value>
    /// Array of AudioSource components for background music.
    /// Each AudioSource corresponds to a specific background track.
    /// </value>
    public AudioSource[] bgm;

	/// <value>
    /// Static reference to the AudioManager instance.
    /// </value>
    public static AudioManager instance;

    // Use this for initialization

	/// <summary>
    /// Initializes the AudioManager component.
    /// Sets up the static instance and ensures the audio manager persists across scenes.
    /// </summary>
    void Start () {
		// Set the static instance to this AudioManager
        instance = this;

		// Ensure the audio manager persists across scenes
        DontDestroyOnLoad(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	}

	/// <summary>
    /// Plays a sound effect based on the given index.
    /// </summary>
    /// <param name="soundToPlay">Index of the sound effect to play.</param>
    public void PlaySFX(int soundToPlay)
    {
		// Check if the requested sound exists in the sfx array
        if (soundToPlay < sfx.Length)
        {
			// Play the selected sound effect
            sfx[soundToPlay].Play();
        }
    }

	/// <summary>
    /// Plays background music based on the given index.
    /// Stops any currently playing background music before starting the new track.
    /// </summary>
    /// <param name="musicToPlay">Index of the background music to play.</param>
    public void PlayBGM(int musicToPlay)
    {
        if (!bgm[musicToPlay].isPlaying)
        {
			// Stop any currently playing background music
            StopMusic();

			// Check if the requested music exists in the bgm array
            if (musicToPlay < bgm.Length)
            {
				// Play the selected background music
                bgm[musicToPlay].Play();
            }
        }
    }

	/// <summary>
    /// Stops all currently playing background music.
    /// </summary>
    public void StopMusic()
    {
		// Iterate through all AudioSource components for background music
        for(int i = 0; i < bgm.Length; i++)
        {
			// Stop each AudioSource component
            bgm[i].Stop();
        }
    }
}
