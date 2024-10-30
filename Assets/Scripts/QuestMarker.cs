using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestMarker : MonoBehaviour {

	/// <value>
    /// The name of the quest being marked.
    /// </value>
    public string questToMark;

	/// <value>
    /// Indicates whether the quest is marked as complete.
    /// </value>
    public bool markComplete;

	 /// <value>
    /// Determines whether the quest marker should be activated when the player enters the trigger area.
    /// </value>
    public bool markOnEnter;

	/// <value>
    /// Private flag to control marking actions.
    /// </value>
    private bool canMark;

	/// <value>
    /// Determines whether the quest marker should be deactivated after marking.
    /// </value>
    public bool deactivateOnMarking;

	// Use this for initialization

	/// <summary>
    /// Called when the script is instantiated.
    /// </summary>
	void Start () {
		
	}
	
	// Update is called once per frame

	/// <summary>
    /// Called every frame after Start().
    /// Handles player interaction with the quest marker.
    /// </summary>
	void Update () {
		if(canMark && Input.GetButtonDown("Fire1"))
        {
            canMark = false;
            MarkQuest();
        }
	}

	/// <summary>
    /// Marks the quest as complete or incomplete based on the markComplete flag.
    /// </summary>
    public void MarkQuest()
    {
        if(markComplete)
        {
            QuestManager.instance.MarkQuestComplete(questToMark);
        } else
        {
            QuestManager.instance.MarkQuestIncomplete(questToMark);
        }

        gameObject.SetActive(!deactivateOnMarking);
    }

	/// <summary>
    /// Called when another Collider enters the trigger area.
    /// </summary>
    /// <param name="other">The Collider that entered the trigger area.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            if (markOnEnter)
            {
                MarkQuest();
            }
            else
            {
                canMark = true;
            }
        }
    }

	/// <summary>
    /// Called when another Collider exits the trigger area.
    /// </summary>
    /// <param name="other">The Collider that exited the trigger area.</param>
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canMark = false;
        }
    }
}
