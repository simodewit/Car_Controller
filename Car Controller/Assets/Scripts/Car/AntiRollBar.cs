using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiRollBar : MonoBehaviour
{
    [SerializeField] private Rigidbody carRb;
    [SerializeField] private WheelController leftWheel;
    [SerializeField] private WheelController rightWheel;
    [SerializeField] private float stiffness;

    public void FixedUpdate()
    {
        AntiRoll();
    }

    private void AntiRoll()
    {
        //check if the car is grounded
        if (!leftWheel.isGrounded || !rightWheel.isGrounded)
        {
            return;
        }

        //calculate the travel of the springs
        float rightTravel = CalculateTravel(rightWheel);
        float leftTravel = CalculateTravel(leftWheel);

        //calculate the force that the ARB should give to the suspension
        float antiRollForce = (rightTravel - leftTravel) * stiffness;

        //add the force to the suspension
        carRb.AddForceAtPosition(rightWheel.transform.up * -antiRollForce, rightWheel.transform.position);
        carRb.AddForceAtPosition(leftWheel.transform.up * antiRollForce, leftWheel.transform.position);
    }

    private float CalculateTravel(WheelController wheel)
    {
        //calculate the spring travel
        float travel = wheel.springTargetPos.localPosition.y - wheel.transform.localPosition.y;
        return travel;
    }
}
