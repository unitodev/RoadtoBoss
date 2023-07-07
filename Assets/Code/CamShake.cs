using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CamShake : MonoBehaviour
{
    public static CamShake Instance { get;private set; }

    private CinemachineVirtualCamera _cinemachineVirtualCamera;

    private float shakeTimer,shakeTimerTotal,startingIntensity;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        _cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public void shakeCam(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
            _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        startingIntensity = intensity;
        shakeTimerTotal = time;
        shakeTimer = time;
    }
    // Update is called once per frame
    void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0)
            {
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin=
                    _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
                Mathf.Lerp(startingIntensity, 0, 1 - (shakeTimer / shakeTimerTotal));
            }
        }
    }
}
