using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class CameraShake : MonoBehaviour
{
    public static Action CameraShaked;
    Camera cam;
    bool isShaking = false;
    
    private void Awake()
    {
        cam = Camera.main;
    }
    private void OnCameraShake()
    {
        if (!isShaking)
        {
            isShaking = true;
            cam.DOShakePosition(0.3f,1f).OnComplete(() => isShaking = false);
        }
    }
    private void OnEnable()
    {
       CameraShaked += OnCameraShake;
    }
    private void OnDisable()
    {
        CameraShaked -= OnCameraShake;
    }
}
