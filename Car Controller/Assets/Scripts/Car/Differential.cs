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

        float leftRPM = leftWheel.rpm;
        float rightRPM = rightWheel.rpm;

        //temp
        leftWheel.motorTorque = entryTorque / 2;
        rightWheel.motorTorque = entryTorque / 2;
    }

    #endregion
}
