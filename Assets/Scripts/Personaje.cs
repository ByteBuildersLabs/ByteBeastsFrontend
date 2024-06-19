using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personaje : MonoBehaviour
{   
    public PersonajeAnimaciones PersonajeAnimaciones { get; private set; }

    private void Awake()
    {
        PersonajeAnimaciones = GetComponent<PersonajeAnimaciones>(); //get the PersonajeAnimaciones component
    }
}
