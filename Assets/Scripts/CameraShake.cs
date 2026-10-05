using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private OrbitCamera orbitCamera;

    private void Awake()
    {
        orbitCamera = GetComponent<OrbitCamera>();
    }
}