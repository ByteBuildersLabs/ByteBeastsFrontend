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

    public string transitionName; // Added this line to store the transition name

    public Vector2 GetDireccionMovimiento => _direccionMovimiento;

    public bool EnMovimiento => _direccionMovimiento.magnitude > 0f;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        // Initialize transitionName if needed
        // transitionName = "YourInitialTransitionName";
    }

    void Update()
    {   
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

        _input = new Vector2(x:horizontal, y:vertical);

        if (_input.x > 0.1f)
        {
            _direccionMovimiento.x = 1f;
        }
        else if (_input.x < 0f)
        {
            _direccionMovimiento.x = -1f;
        }
        else
        {
            _direccionMovimiento.x = 0f;
        }

        if (_input.y > 0.1f)
        {
            _direccionMovimiento.y = 1f;
        }
        else if (_input.y < 0f)
        {
            _direccionMovimiento.y = -1f;
        }
        else
        {
            _direccionMovimiento.y = 0f;
        }
    }

    private void FixedUpdate()
    {
        _rigidbody2D.MovePosition(_rigidbody2D.position + _direccionMovimiento * velocidad * Time.fixedDeltaTime);
    }
}
