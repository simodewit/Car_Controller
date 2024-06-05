using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GearBox : MonoBehaviour
{
    #region variables

    [Tooltip("The script of the engine")]
    [SerializeField] private Engine engine;
    [Tooltip("If this is enabled it will stop you from shifting down into a gear that would create to much rpm's")]
    [SerializeField] private bool shiftAssist = true;
    [Tooltip("The info for every gear")]
    public GearInfo[] gears; // has to be public for the engine

    [HideInInspector] public int currentGear;
    [HideInInspector] public float outputTorque;

    #endregion

    #region update

    public void FixedUpdate()
    {
        CalculateTorque();
    }

    #endregion

    #region shifting

    public void ShiftUp(InputAction.CallbackContext context)
    {
        if (context.performed && currentGear < gears[gears.Length - 1].gearNumber)
        {
            currentGear += 1;
        }
    }

    public void ShiftDown(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }

        if (currentGear == gears[0].gearNumber)
        {
            return;
        }

        if (shiftAssist)
        {
            float afterShiftRPM = engine.rpm / gears[currentGear + 1].gearRatio * gears[currentGear].gearRatio;

            if (afterShiftRPM < engine.maxRPM)
            {
                currentGear -= 1;
            }
            else
            {
                print("you moneyshifted");
            }
        }
        else
        {
            currentGear -= 1;
        }
    }

    #endregion

    #region torque

    private void CalculateTorque()
    {
        outputTorque = engine.outputTorque * gears[currentGear + 1].gearRatio;
    }

    #endregion
}

[Serializable]
public class GearInfo
{
    [Tooltip("The number of the gear that you are in. (reverse = -1, neutral = 0, 1st gear = 1, etc)")]
    public int gearNumber;
    [Tooltip("The amount that the engine torque should be multiplied")]
    public float gearRatio;
}