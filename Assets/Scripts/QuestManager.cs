using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    // Array of quest marker names
    public string[] questMarkerNames;
    
    // Array to track whether each quest is complete
    public bool[] questMarkersComplete;

    // Singleton instance of the QuestManager
    public static QuestManager instance;

    // Initialization
    void Start()
    {
        // Set the singleton instance
        instance = this;

        // Initialize questMarkersComplete based on the number of questMarkerNames
        questMarkersComplete = new bool[questMarkerNames.Length];
    }
    
    // Update is called once per frame
    void Update()
    {
        // Debugging controls
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log(CheckIfComplete("quest test")); // Check if a specific quest is complete
            MarkQuestComplete("quest test"); // Mark a specific quest as complete
            MarkQuestIncomplete("fight the demon"); // Mark a specific quest as incomplete
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            SaveQuestData(); // Save quest data to PlayerPrefs
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            LoadQuestData(); // Load quest data from PlayerPrefs
        }
    }

    // Get the index of a quest in the questMarkerNames array
    public int GetQuestNumber(string questToFind)
    {
        for (int i = 0; i < questMarkerNames.Length; i++)
        {
            if (questMarkerNames[i] == questToFind)
            {
                return i;
            }
        }

        // Log error if quest is not found and return default index
        Debug.LogError("Quest " + questToFind + " does not exist");
        return 0;
    }

    // Check if a specific quest is complete
    public bool CheckIfComplete(string questToCheck)
    {
        int questIndex = GetQuestNumber(questToCheck);
        if (questIndex != 0)
        {
            return questMarkersComplete[questIndex];
        }

        return false;
    }

    // Mark a specific quest as complete
    public void MarkQuestComplete(string questToMark)
    {
        int questIndex = GetQuestNumber(questToMark);
        questMarkersComplete[questIndex] = true;

        // Update any local quest objects that depend on quest completion
        UpdateLocalQuestObjects();
    }

    // Mark a specific quest as incomplete
    public void MarkQuestIncomplete(string questToMark)
    {
        int questIndex = GetQuestNumber(questToMark);
        questMarkersComplete[questIndex] = false;

        // Update any local quest objects that depend on quest completion
        UpdateLocalQuestObjects();
    }

    // Update all quest-related objects in the scene
    public void UpdateLocalQuestObjects()
    {
        // Find all quest-related activator objects in the scene
        QuestObjectActivator[] questObjects = FindObjectsOfType<QuestObjectActivator>();

        if (questObjects.Length > 0)
        {
            for (int i = 0; i < questObjects.Length; i++)
            {
                // Check completion status for each quest object
                questObjects[i].CheckCompletion();
            }
        }
    }

    // Save quest data to PlayerPrefs
    public void SaveQuestData()
    {
        for (int i = 0; i < questMarkerNames.Length; i++)
        {
            if (questMarkersComplete[i])
            {
                PlayerPrefs.SetInt("QuestMarker_" + questMarkerNames[i], 1);
            }
            else
            {
                PlayerPrefs.SetInt("QuestMarker_" + questMarkerNames[i], 0);
            }
        }
    }

    // Load quest data from PlayerPrefs
    public void LoadQuestData()
    {
        for (int i = 0; i < questMarkerNames.Length; i++)
        {
            int valueToSet = 0;
            if (PlayerPrefs.HasKey("QuestMarker_" + questMarkerNames[i]))
            {
                valueToSet = PlayerPrefs.GetInt("QuestMarker_" + questMarkerNames[i]);
            }

            // Set the quest marker status based on saved data
            questMarkersComplete[i] = valueToSet == 1;
        }
    }
}
