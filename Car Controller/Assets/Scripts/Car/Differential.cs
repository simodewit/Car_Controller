using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Differential : MonoBehaviour
{
    #region variables

    [Header("Wheels")]
    [Tooltip("The rear left wheelController")]
    [SerializeField] private Tyre leftWheel;
    [Tooltip("The rear right wheelController")]
    [SerializeField] private Tyre rightWheel;

    [Header("Differential")]
    [Tooltip("The gearbox script")]
    [SerializeField] private GearBox gearBox;
    [Tooltip("The final gear ratio of the car")]
    public float finalGearRatio; // has to be public for the engine script
    [Tooltip("How much of the differential can be opened or closed"), Range(0, 100)]
    [SerializeField] private float diffLock;

    private int amountOfDifferentials;

    #endregion

    #region start and update

    public void Start()
    {
        Differential[] diffs = FindObjectsOfType<Differential>();
        amountOfDifferentials = diffs.Length;
    }

    public void FixedUpdate()
    {
        Diff();
    }

    #endregion

    #region differential

    private void Diff()
    {
        //calculate the amount of torque before dividing
        float entryTorque = gearBox.outputTorque * finalGearRatio / amountOfDifferentials;

        //calculate the percentages
        float average = (leftWheel.rpm + rightWheel.rpm) / 2;
        float leftFactor = leftWheel.rpm / average;
        float rightFactor = rightWheel.rpm / average;

        //calculate the min and max for the clamp
        float min = (100 - diffLock / 2) / 100;
        float max = (100 + diffLock / 2) / 100;

        //clamp the vallues
        leftFactor = Mathf.Clamp(leftFactor, min, max);
        rightFactor = Mathf.Clamp(rightFactor, min, max);

        //temp
        leftWheel.motorTorque = entryTorque / 2 * leftFactor;
        rightWheel.motorTorque = entryTorque / 2 * rightFactor;
    }

    #endregion
}
