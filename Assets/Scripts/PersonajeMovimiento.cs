using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonajeMovimiento : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    private Vector2 _input;
    private Vector2 _direccionMovimiento;

    [SerializeField] private float velocidad;

    [SerializeField] Joystick joystick;

    public Vector2 GetDireccionMovimiento => _direccionMovimiento;

    public bool EnMovimiento => _direccionMovimiento.magnitude > 0f;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   

        //float horizontal = Input.GetAxisRaw("Horizontal");
        //float vertical = Input.GetAxisRaw("Vertical");

        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

        _input = new Vector2(x:horizontal, y:vertical);

        // Direcciones con X
        
        if (_input.x > 0.1f) //si el input es mayor a 0.1f entonces se mueve a la derecha
        {
            _direccionMovimiento.x = 1f;
        }
        else if (_input.x < 0f) //si el input es menor a 0 entonces se mueve a la izquierda
        {
            _direccionMovimiento.x = -1f;
        }
        else{ //no se mueve horizontalmente
            _direccionMovimiento.x = 0f;
        }

        //Direcciones con Y

        if (_input.y > 0.1f) //si el input es mayor a 0.1f entonces se mueve a hacia arriba
        {
            _direccionMovimiento.y = 1f;
        }
        else if (_input.y < 0f) //si el input es menor a 0 entonces se mueve hacia abajo
        {
            _direccionMovimiento.y = -1f;
        }
        else{ //no se mueve verticalmente
            _direccionMovimiento.y = 0f;
        }
    }

    //para mover al personaje

    private void FixedUpdate()
    {
        _rigidbody2D.MovePosition(_rigidbody2D.position + _direccionMovimiento * velocidad * Time.fixedDeltaTime);
    }
}