﻿
using System;
using UnityEngine;

public class RearviewCameraBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject cameraView;
    private static int cameraRequests;

    void Start()
    {
        cameraRequests = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(cameraRequests);
        cameraView.SetActive(cameraRequests > 0);
    }

    public static void RequestRearviewOn()
    {
        cameraRequests++;
        cameraRequests = Math.Max(cameraRequests, 1);
    }

    public static void RequestRearviewOff()
    {
        cameraRequests--;
    }
}
