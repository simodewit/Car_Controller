using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Engine : MonoBehaviour
{
    #region variables

    [Tooltip("The script of the gearbox")]
    [SerializeField] private GearBox gearBox;
    [Tooltip("The script of the throttle body")]
    [SerializeField] private ThrottleBody throttleBody;
    [Tooltip("The fuel tank script")]
    [SerializeField] private FuelTank fuelTank;

    [Tooltip("The maximum rpm that the engine can run"), Range(0, 25000)]
    public int maxRPM = 5000; //has to be public for the gearbox
    [Tooltip("The total newton meters of torque that the car has"), Range(0, 5000)]
    [SerializeField] private float torque = 200;
    [Tooltip("The torque given at specific moments"), Curve(0, 0, 1f, 1f, true)]
    [SerializeField] private AnimationCurve torqueCurve;
    [Tooltip("The amount of fuel used per 1l air in mililiter"), Range(0, 10)]
    [SerializeField] private float fuelUsage;

    //get variables
    [Tooltip("The rpm of the engine")]
    /*[HideInInspector]*/ public float rpm;
    [Tooltip("The output torque of the engine onto the gearbox")]
    [HideInInspector] public float outputTorque;

    #endregion

    #region update and input

    public void FixedUpdate()
    {
        FuelConsumtion();
        UpdateRPM();
        CalculateTorque();
    }

    #endregion

    #region fuel consumption

    private void FuelConsumtion()
    {
        float fuelUsed = throttleBody.air * fuelUsage * 0.001f / 50;
        fuelTank.fuel -= fuelUsed;
    }

    #endregion

    #region RPM

    private void UpdateRPM()
    {
        float rpmToAdd = 0;



        //rpm = rpmToAdd;
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

        if (rpm > maxRPM)
        {
            totalTorque = 0;
        }
        else
        {
            totalTorque = torqueOutput;
        }

        //apply the torque
        outputTorque = totalTorque;
    }

    #endregion
}
