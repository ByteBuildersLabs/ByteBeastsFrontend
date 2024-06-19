using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonajeAnimaciones : MonoBehaviour
{   
    private Animator _animator;
    private PersonajeMovimiento _personajeMovimiento; //referencia al script PersonajeMovimiento

    [SerializeField] private string layerIdle; //nombre de la capa de animacion idle

    [SerializeField] private string layerCaminar; //nombre de la capa de animacion movimiento

    private readonly int direccionX = Animator.StringToHash("X"); //hash de la variable X del animator para optimizar el acceso
    private readonly int direccionY = Animator.StringToHash("Y"); //hash de la variable Y del animator para optimizar el acceso

    //Metodo awake permite inicializar variables antes de que empiece el juego
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _personajeMovimiento = GetComponent<PersonajeMovimiento>();
    }

    // Update is called once per frame
    void Update()
    {    
        UpdateLayer();
        
        //si el personaje se esta moviendo se actualiza la animacion
        if (_personajeMovimiento.EnMovimiento)
        {
            _animator.SetFloat(direccionX, _personajeMovimiento.GetDireccionMovimiento.x);
            _animator.SetFloat(direccionY, _personajeMovimiento.GetDireccionMovimiento.y);
        }
    }

    private void ActivateLayer(string layer){

        //To desactivate all layers
        for (int i = 0; i < _animator.layerCount; i++)
        {
            _animator.SetLayerWeight(i, 0); //Weight 0 means desactivate and 1 means activate
        }

        _animator.SetLayerWeight(_animator.GetLayerIndex(layer), 1); //Activate the layer using the name
    }

    private void UpdateLayer()
    {
        if (_personajeMovimiento.EnMovimiento) //if the character is moving
        {
            ActivateLayer(layerCaminar); //Activate the layer of movement
        }
        else
        {
            ActivateLayer(layerIdle); //Activate the layer of idle
        }
    }
}