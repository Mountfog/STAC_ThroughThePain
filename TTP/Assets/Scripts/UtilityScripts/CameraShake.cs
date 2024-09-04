using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeMagnitude = 0.2f;
    public float shakeDuration = 0.5f;
    public float dampingSpeed = 1.0f;

    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.localPosition;
    }


    public void ShakeCamera()
    {
        Camera.main.DOShakePosition(0.1f, 0.1f);
    }

}
