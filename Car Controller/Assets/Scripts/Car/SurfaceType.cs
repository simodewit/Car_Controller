using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceType : MonoBehaviour
{
    [Tooltip("The amount of grip on this surface in percentages"), Range(0f, 100f)]
    public float grip = 100;
}
