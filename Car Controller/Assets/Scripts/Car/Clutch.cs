using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Clutch : MonoBehaviour
{
    [SerializeField] private Engine engine;

    [HideInInspector] public float outputTorque;
    [HideInInspector] public float clutchAxis;

    public void FixedUpdate()
    {
        outputTorque = engine.outputTorque * (1 - clutchAxis);
    }

    public void ClutchInput(InputAction.CallbackContext context)
    {
        clutchAxis = context.ReadValue<float>();
    }
}
