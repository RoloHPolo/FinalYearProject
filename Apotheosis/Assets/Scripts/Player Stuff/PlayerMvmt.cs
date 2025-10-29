using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMvmt : MonoBehaviour
{
    [Header("Movement")]
    public float mvSpd;
    public float drag;

    [Header("Ground Check")]
    public float height;
    public LayerMask ground;
    bool grounded;

    public Transform orient;

    float horizontalInput;
    float verticalInput;

    Vector3 mvDir;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        //check ground with downward raycast
        grounded = Physics.Raycast(transform.position, Vector3.down, height * 0.5f + 0.2f, ground);

        GetInput();
        ControlSpeed();

        //drag
        if (grounded)
            rb.drag = drag;
        else 
            rb.drag = 0;
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

    }

    private void Movement()
    {
        mvDir = orient.forward * verticalInput + orient.right * horizontalInput;
        rb.AddForce(mvDir * mvSpd * 10f, ForceMode.Force);
    }

    private void ControlSpeed()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //limit velocity
        if (flatVel.magnitude > mvSpd)
        {
            Vector3 limitedVel = flatVel.normalized * mvSpd;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }
}
