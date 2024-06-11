using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TrackSelect
{
    nationals,
    GP,
}

public class TrackSelection : MonoBehaviour
{
    [SerializeField] private TrackInfo[] trackInfo;

    public void ChangeTrack(TrackSelect selection)
    {
        foreach (TrackInfo info in trackInfo)
        {
            if (info.selection == selection)
            {
                info.objectsToEnable.SetActive(true);
            }
            else
            {
                info.objectsToEnable.SetActive(false);
            }
        }
    }
}

[Serializable]
public class TrackInfo
{
    public TrackSelect selection;
    public GameObject objectsToEnable;
}