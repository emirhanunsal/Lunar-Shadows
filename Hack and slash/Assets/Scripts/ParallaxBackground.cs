using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float BackgroundLayerNumberFrontisHigher;

    private Vector3 previousPlayerPosition;

    void Start()
    {
        if (playerTransform != null)
        {
            previousPlayerPosition = playerTransform.position;
        }
        else
        {
            Debug.LogError("Player Transform is not assigned in the ParallaxBackground script.");
        }
    }

    void LateUpdate()
    {
        if (playerTransform != null)
        {
            Vector3 deltaMovement = playerTransform.position - previousPlayerPosition;
            float parallax = deltaMovement.x * BackgroundLayerNumberFrontisHigher;
            transform.position += new Vector3(parallax/100, 0f, 0f);
            previousPlayerPosition = playerTransform.position;
        }
    }
}