using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Camera cam;
    public float xSensitivity;
    public float ySensitivity;
    public GunSystem Gun;

    public Transform orientation;

    float xRotation;
    float yRotation;

    public void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.smoothDeltaTime * xSensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.smoothDeltaTime * ySensitivity;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        if (Gun.readyToShoot == true)
        {
            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        }
       
    }


}