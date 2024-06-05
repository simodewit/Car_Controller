using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : MonoBehaviour
{
    #region variables

    [Tooltip("The script of the gearbox")]
    [SerializeField] private GearBox gearBox;
    [Tooltip("The script of the throttle body")]
    [SerializeField] private ThrottleBody throttleBody;
    [Tooltip("The fuel tank script")]
    [SerializeField] private FuelTank fuelTank;
    [Tooltip("The clutch script")]
    [SerializeField] private Clutch clutch;

    [Tooltip("The maximum rpm that the engine can run"), Range(0, 25000)]
    public int maxRPM = 5000; //has to be public for the gearbox
    [Tooltip("The rpm's that it has to be under in order to stop cutting the fuel for the engine"), Range(0, 25000)]
    public int cutRPM = 4500; //has to be public for the gearbox
    [Tooltip("The engine capacity in liters"), Range(0, 20)]
    public float engineCapacity; // has to be public for the throttleBody
    [Tooltip("The amount of cylinders that the engine has"), Range(0, 30)]
    public int cylinders; // has to be public for the throttleBody
    [Tooltip("fuel to air ratio. this ratio is ideal at 14,7"), Range(0, 30)]
    [SerializeField] private float fuelAirRatio = 14.7f;
    [Tooltip("How efficient the motor is with converting fuel to power"), Range(0, 100)]
    [SerializeField] private float engineEfficiency = 25;
    [Tooltip("The amount of friction in the engine per rpm")]
    [SerializeField] private float engineFriction;
    [Tooltip("How hard it is for the engine to get the piston movement going circles per rpm")]
    [SerializeField] private float inertia;
    [Tooltip("The radius of the crankshaft")]
    [SerializeField] private float crankDiameter;

    //get variables
    [Tooltip("The rpm of the engine")]
    [HideInInspector] public float rpm;
    [Tooltip("The output torque of the engine onto the gearbox")]
    [HideInInspector] public float outputTorque;

    private float usedFuel;
    private float fuelToCut;

    #endregion

    #region update and input

    public void FixedUpdate()
    {
        FuelConsumtion();
        CalculateTorque();
        CalculateRPM();
    }

    #endregion

    #region fuel consumption

    private void FuelConsumtion()
    {
        //calculate the air per cylinder and per cycle
        float airPerCycle = throttleBody.air / 4;
        float airPerCylinder = airPerCycle / cylinders;

        //calculate the amount of air per frame
        float airPerFrame = airPerCylinder / 50;

        //calculate the fuel used this frame
        float fuelUsage = airPerFrame / fuelAirRatio * fuelToCut;

        //apply the fuel
        fuelTank.fuel -= fuelUsage;
        usedFuel = fuelUsage;
    }

    #endregion

    #region calculate torque

    private void CalculateTorque()
    {
        //calculate the amount of joules per mililiter
        float joules = fuelTank.energyContent;
        float joulesPerMl = joules * (engineEfficiency / 100);

        //calculate the amount of joules from the used fuel
        float mlOfFuel = usedFuel * 1000 * 50; // times 1000 to go to liters again and * 50 for something (idk what)
        float totalEnergy = joulesPerMl * mlOfFuel;

        //calculate the amount of torque produced by the engine
        float nmOutput = totalEnergy / 1 * cylinders;

        //calculate the inertia and friction factors
        float friction = engineFriction * rpm;
        float rotateResistance = Mathf.Sqrt(inertia * rpm);

        //add the friction and inertia factors
        nmOutput -= rotateResistance + friction;

        outputTorque = nmOutput;
    }

    #endregion

    #region RPM

    private void CalculateRPM()
    {
        //apply the rpm's
        rpm += (RPMWithClutch() * clutch.clutchAxis) + (RPMWithoutClutch() * (1 - clutch.clutchAxis));

        //stop fuel consumption when over the redline
        if (rpm > maxRPM)
        {
            fuelToCut = 0;
        }
        if (rpm < cutRPM)
        {
            fuelToCut = 1;
        }
    }

    private float RPMWithClutch()
    {
        //calculate the amount of resistance and friction
        float rotateResistance = Mathf.Sqrt(inertia * rpm);
        float totalEngineFriction = engineFriction * rpm;

        //calculate the amount of torque that the engine made
        float totalTorque = outputTorque - (rotateResistance + totalEngineFriction);

        //calculate the circumfrence
        float crankshaftCir = crankDiameter * Mathf.PI;

        //calculate the amount of rpm to add
        float rpmToAdd = totalTorque / crankshaftCir;

        return rpmToAdd;
    }

    private float RPMWithoutClutch()
    {
        //calculate the amount of resistance and friction
        float rotateResistance = Mathf.Sqrt(inertia * rpm);
        float totalEngineFriction = engineFriction * rpm;

        //calculate the amount of torque that the engine made
        float totalTorque = outputTorque - (rotateResistance + totalEngineFriction);

        //calculate the circumfrence
        float crankshaftCir = crankDiameter * Mathf.PI;

        //calculate the amount of rpm to add
        float rpmToAdd = totalTorque / crankshaftCir;

        return rpmToAdd;
    }

    #endregion
}
