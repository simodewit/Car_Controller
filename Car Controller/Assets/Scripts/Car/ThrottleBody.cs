using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThrottleBody : MonoBehaviour
{
    #region variables

    [Tooltip("The rigidbody of the car")]
    [SerializeField] private Rigidbody carRb;
    [Tooltip("The engine script")]
    [SerializeField] private Engine engine;
    [Tooltip("The engine capacity in liters"),Range(0,20)]
    [SerializeField] private float engineCapacity;
    [Tooltip("The deadzone in the pedal before the throttle is used"), Range(0, 100)]
    [SerializeField] private float throttleDeadzone = 0;

    //get variables
    [Tooltip("The amount of air intake in liters")]
    [HideInInspector] public float air;

    //private variables
    private float throttleAxis;

    #endregion

    #region update and input

    public void FixedUpdate()
    {
        Throttle();
    }

    public void ThrottleInput(InputAction.CallbackContext context)
    {
        throttleAxis = context.ReadValue<float>();
    }

    #endregion

    #region throttle

    private void Throttle()
    {
        float axis = throttleAxis;

        if (axis < throttleDeadzone)
        {
            axis = 0;
        }

        float velocity = carRb.velocity.z;

        if (velocity < 0)
        {
            velocity = 0;
        }

        air = 0.5f * engineCapacity * engine.rpm * axis * (velocity / 100);
    }

    #endregion
}
