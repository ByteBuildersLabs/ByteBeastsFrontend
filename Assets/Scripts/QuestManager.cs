using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages quests and quest markers in the game.
/// Provides functionality for checking quest completion, marking quests as complete/incomplete,
/// saving/loading quest data, and updating local quest objects.
/// </summary>
public class QuestManager : MonoBehaviour {

	/// <summary>
    /// Array of quest marker names.
    /// </summary>
    public string[] questMarkerNames;

	/// <summary>
    /// Array to track the completion status of quest markers.
    /// </summary>
    public bool[] questMarkersComplete;

	/// <summary>
    /// Singleton instance of the QuestManager.
    /// </summary>
    public static QuestManager instance;

	// Use this for initialization

	/// <summary>
    /// Called when the script is instantiated.
    /// Initializes the singleton instance and sets up quest marker tracking.
    /// </summary>
	void Start () {
        instance = this;

        questMarkersComplete = new bool[questMarkerNames.Length];
	}
	
	// Update is called once per frame

	/// <summary>
    /// Called every frame after Start().
    /// Provides keyboard shortcuts for testing quest functionality.
    /// </summary>
	void Update () {
		if(Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log(CheckIfComplete("quest test"));
            MarkQuestComplete("quest test");
            MarkQuestIncomplete("fight the demon");
        }

        if(Input.GetKeyDown(KeyCode.O))
        {
            SaveQuestData();
        }

        if(Input.GetKeyDown(KeyCode.P))
        {
            LoadQuestData();
        }
	}

	/// <summary>
    /// Retrieves the index of a quest marker in the questMarkerNames array.
    /// </summary>
    /// <param name="questToFind">Name of the quest marker to find.</param>
    /// <returns>The index of the quest marker if found, otherwise 0.</returns>
    public int GetQuestNumber(string questToFind)
    {
        for(int i = 0; i < questMarkerNames.Length; i++)
        {
            if(questMarkerNames[i] == questToFind)
            {
                return i;
            }
        }

        Debug.LogError("Quest " + questToFind + " does not exist");
        return 0;
    }

	/// <summary>
    /// Checks if a specific quest is marked as complete.
    /// </summary>
    /// <param name="questToCheck">Name of the quest to check.</param>
    /// <returns>True if the quest is complete, false otherwise.</returns>
    public bool CheckIfComplete(string questToCheck)
    {
        if(GetQuestNumber(questToCheck) != 0)
        {
            return questMarkersComplete[GetQuestNumber(questToCheck)];
        }

        return false;
    }

	/// <summary>
    /// Marks a quest as complete.
    /// </summary>
    /// <param name="questToMark">Name of the quest to mark as complete.</param>
    public void MarkQuestComplete(string questToMark)
    {
        questMarkersComplete[GetQuestNumber(questToMark)] = true;

        UpdateLocalQuestObjects();
    }

	/// <summary>
    /// Marks a quest as incomplete.
    /// </summary>
    /// <param name="questToMark">Name of the quest to mark as incomplete.</param>
    public void MarkQuestIncomplete(string questToMark)
    {
        questMarkersComplete[GetQuestNumber(questToMark)] = false;

        UpdateLocalQuestObjects();
    }

	/// <summary>
    /// Updates local quest objects based on the completion status of quest markers.
    /// </summary>
    public void UpdateLocalQuestObjects()
    {
        QuestObjectActivator[] questObjects = FindObjectsOfType<QuestObjectActivator>();

        if(questObjects.Length > 0)
        {
            for(int i = 0; i < questObjects.Length; i++)
            {
                questObjects[i].CheckCompletion();
            }
        }
    }

	/// <summary>
    /// Saves quest completion status to PlayerPrefs.
    /// </summary>
    public void SaveQuestData()
    {
        for(int i = 0; i < questMarkerNames.Length; i++)
        {
            if(questMarkersComplete[i])
            {
                PlayerPrefs.SetInt("QuestMarker_" + questMarkerNames[i], 1);
            } else
            {
                PlayerPrefs.SetInt("QuestMarker_" + questMarkerNames[i], 0);
            }
        }
    }

	/// <summary>
    /// Loads quest completion status from PlayerPrefs.
    /// </summary>
    public void LoadQuestData()
    {
        for(int i = 0; i < questMarkerNames.Length; i++)
        {
            int valueToSet = 0;
            if(PlayerPrefs.HasKey("QuestMarker_" + questMarkerNames[i]))
            {
                valueToSet = PlayerPrefs.GetInt("QuestMarker_" + questMarkerNames[i]);
            }

            if(valueToSet == 0)
            {
                questMarkersComplete[i] = false;
            } else
            {
                questMarkersComplete[i] = true;
            }
        }
    }
}
