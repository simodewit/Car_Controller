using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Differential : MonoBehaviour
{
    [Header("Wheels")]
    [Tooltip("The rear left wheelController")]
    [SerializeField] private WheelController leftWheel;
    [Tooltip("The rear right wheelController")]
    [SerializeField] private WheelController rightWheel;

    [Header("Differential")]
    [Tooltip("The gearbox script")]
    [SerializeField] private GearBox gearBox;
    [Tooltip("The final gear ratio of the car")]
    [SerializeField] private float finalGearRatio;
    [Tooltip("How much of the differential can be opened or closed"), Range(0, 100)]
    [SerializeField] private float diffLock;

    public void FixedUpdate()
    {
        Diff();
    }

    private void Diff()
    {
        //calculate the amount of torque before dividing
        float entryTorque = gearBox.outputTorque * finalGearRatio;

        float leftRPM = leftWheel.rpm;
        float rightRPM = rightWheel.rpm;

        //temp
        leftWheel.motorTorque = entryTorque / 2;
        rightWheel.motorTorque = entryTorque / 2;
    }
}
