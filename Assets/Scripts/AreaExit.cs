using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaExit : MonoBehaviour {

    public string areaToLoad;

    public string areaTransitionName;

    public AreaEntrance theEntrance;

    public float waitToLoad = 1f;
    private bool shouldLoadAfterFade;

    private PersonajeMovimiento personajeMovimiento;

    void Start () {
        theEntrance.transitionName = areaTransitionName;
        personajeMovimiento = FindObjectOfType<PersonajeMovimiento>();
    }
    
    void Update () {
        if(shouldLoadAfterFade)
        {
            waitToLoad -= Time.deltaTime;
            if(waitToLoad <= 0)
            {
                shouldLoadAfterFade = false;
                SceneManager.LoadScene(areaToLoad);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            shouldLoadAfterFade = true;

            if (UIFade.instance != null)
            {
                UIFade.instance.FadeToBlack();
            }
            else
            {
                Debug.LogWarning("UIFade instance not found.");
            }

            if (personajeMovimiento != null)
            {
                personajeMovimiento.transitionName = areaTransitionName;
            }
            else
            {
                Debug.LogWarning("PersonajeMovimiento instance not found.");
            }
        }
    }
}
