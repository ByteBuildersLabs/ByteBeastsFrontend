using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestObjectActivator : MonoBehaviour {

    // The GameObject that will be activated or deactivated based on quest completion
    public GameObject objectToActivate;

    // The name of the quest to check for completion
    public string questToCheck;

    // If true, the object will be activated when the quest is complete. If false, it will be deactivated.
    public bool activeIfComplete;

    // Flag to ensure the quest check is done only once
    private bool initialCheckDone;

    // Use this for initialization
    void Start () {
        // Initialization code here if needed
    }
    
    // Update is called once per frame
    void Update () {
        // Check the quest completion status only once
        if(!initialCheckDone)
        {
            initialCheckDone = true;

            // Perform the quest completion check
            CheckCompletion();
        }
    }

    // Method to check if the specified quest is complete and activate/deactivate the object accordingly
    public void CheckCompletion()
    {
        // Check if the quest is complete using QuestManager
        if(QuestManager.instance.CheckIfComplete(questToCheck))
        {
            // Activate or deactivate the object based on quest completion
            objectToActivate.SetActive(activeIfComplete);
        }
    }
}
