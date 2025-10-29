using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControl : MonoBehaviour
{
    //linked to empty game object attached to the player, just stores which way player facing
    public Transform orient;

    public float xSens;
    public float ySens;
    float xRot;
    float yRot;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        //get mouse input
        float xCursor = Input.GetAxisRaw("Mouse X") * xSens * Time.deltaTime;
        float yCursor = Input.GetAxisRaw("Mouse Y") * ySens * Time.deltaTime;

        yRot += xCursor;
        xRot -= yCursor;

        //lock cam to not go further than 90 deg up or down
        xRot = Mathf.Clamp(xRot, -90f, 90f);

        //rotate cam and player orientation
        transform.rotation = Quaternion.Euler(xRot, yRot, 0);
        orient.rotation = Quaternion.Euler(0, yRot, 0);
    }


}
