using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILookAtCam : MonoBehaviour
{
    // reference ke cameranya
    [SerializeField] private Transform cam;

    private void Start()
    {
        cam = Camera.main.transform;
    }

    private void LateUpdate()
    {
        // menghadap ke cam
        transform.LookAt(transform.position + cam.forward);
    }
}
