using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        camera.aspect = 1681.0f / 882.0f;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void Awake()
    {
        camera.aspect = 1681.0f / 882.0f;
    }
}
