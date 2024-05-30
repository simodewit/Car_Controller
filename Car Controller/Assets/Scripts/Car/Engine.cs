using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Engine : MonoBehaviour
{
    #region variables

    [Tooltip("The deadzone in the pedal before the throttle is used"), Range(0, 100)]
    [SerializeField] private float throttleDeadzone = 0;
    [Tooltip("The maximum rpm that the engine can run"), Range(0, 25000)]
    public int maxRPM = 5000; //has to be public for the gearbox
    [Tooltip("The maximum rpm that the engine can run"), /*Range(0, 3)*/]
    [SerializeField] private float inertia = 0.1f;
    [Tooltip("The total newton meters of torque that the car has"), Range(0, 5000)]
    [SerializeField] private float torque = 200;
    [Tooltip("The torque given at specific moments"), Curve(0, 0, 1f, 1f, true)]
    [SerializeField] private AnimationCurve torqueCurve;

    //get variables
    [HideInInspector] public float rpm;
    [HideInInspector] public float outputTorque;

    //private variables
    private float throttleAxis;

    #endregion

    #region update and input

    public void FixedUpdate()
    {
        UpdateRPM();
        CalculateTorque();
    }

    public void ThrottleInput(InputAction.CallbackContext context)
    {
        throttleAxis = context.ReadValue<float>();
    }

    #endregion

    #region hard lerp

    public float HardLerp(float current, float end, float speed)
    {
        // calculate the difference between current and end values
        float difference = end - current;

        // calculate the increment based on speed
        float add = speed * Time.fixedDeltaTime;

        // clamp the value so it doesnt overshoot
        add = Mathf.Clamp(add, -Mathf.Abs(difference), Mathf.Abs(difference));

        //return the value
        return current + add;
    }

    #endregion

    #region RPM

    private void UpdateRPM()
    {
        // calculate ideal rpm
        float targetRPM = throttleAxis * maxRPM;

        if (rpm < targetRPM && rpm < maxRPM)
        {
            // lerp rpm to ideal rpm when accelerating the rpm's
            rpm = Mathf.Lerp(rpm, targetRPM, inertia);
        }
        else if(rpm > targetRPM || rpm > maxRPM)
        {
            // lerp rpm to ideal rpm when decelerating the rpm's
            rpm = Mathf.Lerp(rpm, targetRPM, inertia);
        }

        print(rpm);
    }

    #endregion

    #region torque

    private void CalculateTorque()
    {
        float totalTorque = 0;

        //calculate the amount of torque from the curve
        float placeInCurve = rpm / maxRPM;
        float curveMultiplier = torqueCurve.Evaluate(placeInCurve);
        float torqueOutput = torque * curveMultiplier;

        if (throttleAxis > throttleDeadzone)
        {
            totalTorque = torqueOutput * throttleAxis;
        }

        //apply the torque
        outputTorque = totalTorque;
    }

    #endregion
}
