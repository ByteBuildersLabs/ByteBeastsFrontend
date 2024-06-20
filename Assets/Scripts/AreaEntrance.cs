using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEntrance : MonoBehaviour {

    public string transitionName;

    // Reference to PersonajeMovimiento instance
    private PersonajeMovimiento personajeMovimiento;

    void Start () {
        // Find the instance of PersonajeMovimiento in the scene
        personajeMovimiento = FindObjectOfType<PersonajeMovimiento>();

        // If the transitionName matches, set the position of PersonajeMovimiento
        if(personajeMovimiento != null && transitionName == personajeMovimiento.transitionName)
        {
            personajeMovimiento.transform.position = transform.position;
        }

        UIFade.instance.FadeFromBlack();
        // If you need to manage fading state, you can do it here
        // but it has been removed because GameManager is not used.
    }
    
    void Update () {
        // Add any additional logic if needed
    }
}
