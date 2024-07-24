using System.Collections;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    private CinemachineVirtualCamera CinemachineVirtualCamera;
    private CinemachineBasicMultiChannelPerlin cbmcp;

    [SerializeField] private float shakeIntensity;
    [SerializeField] private float shakeTime;

    private void Awake()
    {
        CinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        cbmcp = CinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        StopShake();
    }

    public void ShakeCamera()
    {
        StopAllCoroutines();  // Stop any ongoing shake coroutine
        StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        cbmcp.m_AmplitudeGain = shakeIntensity;
        yield return new WaitForSeconds(shakeTime);
        StopShake();
    }

    public void StopShake()
    {
        cbmcp.m_AmplitudeGain = 0f;
    }
}