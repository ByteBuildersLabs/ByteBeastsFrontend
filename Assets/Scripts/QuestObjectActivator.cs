using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Activates a GameObject based on the completion status of a quest.
/// </summary>
public class QuestObjectActivator : MonoBehaviour {

	/// <summary>
    /// The GameObject to activate/deactivate based on quest completion.
    /// </summary>
    public GameObject objectToActivate;

	/// <summary>
    /// The quest identifier to check for completion.
    /// </summary>
    public string questToCheck;

	/// <summary>
    /// Determines whether to activate the object if the quest is complete.
    /// </summary>
    public bool activeIfComplete;

	/// <summary>
    /// Flag to prevent repeated checks in Update().
    /// </summary>
    private bool initialCheckDone;

	// Use this for initialization
	 /// <summary>
    /// Called when the script is instantiated.
    /// Initializes the initial check flag.
    /// </summary>
	void Start () {
		
	}
	
	// Update is called once per frame

	/// <summary>
    /// Called every frame after Start().
    /// Checks if the quest is complete and activates/deactivates the associated object.
    /// </summary>
	void Update () {
		if(!initialCheckDone)
        {
            initialCheckDone = true;

            CheckCompletion();
        }
	}

	/// <summary>
    /// Checks if the specified quest is complete and activates/deactivates the associated object accordingly.
    /// </summary>
    public void CheckCompletion()
    {
        if(QuestManager.instance.CheckIfComplete(questToCheck))
        {
            objectToActivate.SetActive(activeIfComplete);
        }
    }
}
