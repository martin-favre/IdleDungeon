using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float translationSpeed = 1f;
    public float lookSpeedH = 4f;
    public float lookSpeedV = 4f;
    public float zoomSpeed = 4f;
    public float dragSpeed = 12f;

    float yaw = 0;
    float pitch = 0;

    Camera mCamera;
    // Start is called before the first frame update
    void Start()
    {
        mCamera = GetComponent<Camera>();
        pitch = transform.eulerAngles.x;
        yaw = transform.eulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        handleTranslationMovement();
        handleRotation();
    }

    void handleRotation()
    {
        //Look around with Right Mouse
        if (Input.GetMouseButton(1))
        {
            yaw += lookSpeedH * Input.GetAxis("Mouse X");
            pitch -= lookSpeedV * Input.GetAxis("Mouse Y");

            transform.eulerAngles = new Vector3(pitch, yaw, 0f);
        }

        //drag camera around with Middle Mouse
        if (Input.GetMouseButton(2))
        {
            transform.Translate(-Input.GetAxisRaw("Mouse X") * Time.deltaTime * dragSpeed, -Input.GetAxisRaw("Mouse Y") * Time.deltaTime * dragSpeed, 0);
        }

        //Zoom in and out with Mouse Wheel
        transform.Translate(0, 0, Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, Space.Self);

    }

    void handleTranslationMovement()
    {
        Vector3 movementDirection = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) movementDirection += new Vector3(0, 0, 1);
        if (Input.GetKey(KeyCode.A)) movementDirection += new Vector3(-1, 0, 0);
        if (Input.GetKey(KeyCode.S)) movementDirection += new Vector3(0, 0, -1);
        if (Input.GetKey(KeyCode.D)) movementDirection += new Vector3(1, 0, 0);
        translateCamera(movementDirection);
    }

    void translateCamera(Vector3 dir)
    {
        if (mCamera)
        {
            mCamera.transform.Translate(dir * translationSpeed);
        }
    }
}