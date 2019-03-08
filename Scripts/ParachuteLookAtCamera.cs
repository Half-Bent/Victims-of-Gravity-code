using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParachuteLookAtCamera : MonoBehaviour
{

    private Transform CameraTransform;

    // Start is called before the first frame update
    void Start()
    {
        CameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(CameraTransform);
    }
}
