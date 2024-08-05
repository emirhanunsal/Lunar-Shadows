using System.Collections;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private CinemachineBasicMultiChannelPerlin cbmcp;

    [SerializeField] private float shakeIntensity;
    [SerializeField] private float shakeTime;

    private void Awake()
    {
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        if (cinemachineVirtualCamera != null)
        {
            cbmcp = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }
        StopShake();
    }

    public void ShakeCamera()
    {
        if (cinemachineVirtualCamera != null && cinemachineVirtualCamera.gameObject.activeInHierarchy)
        {
            StopAllCoroutines();
            StartCoroutine(Shake());
        }
    }

    private IEnumerator Shake()
    {
        if (cbmcp != null)
        {
            cbmcp.m_AmplitudeGain = shakeIntensity;
            yield return new WaitForSeconds(shakeTime);
            StopShake();
        }
    }

    public void StopShake()
    {
        if (cbmcp != null)
        {
            cbmcp.m_AmplitudeGain = 0f;
        }
    }
}
