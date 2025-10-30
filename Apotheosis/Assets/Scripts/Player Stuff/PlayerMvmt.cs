using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMvmt : MonoBehaviour
{
    [Header("Movement")]
    private float mvSpd;
    public float walkSpd;
    public float sprintSpd;
    public float drag;

    public float jumpForce;
    public float jumpCD;
    public float airMulti;
    bool canJump;

    [Header("Keybinds")]
    public KeyCode jumpBind = KeyCode.Space;
    public KeyCode sprintBind = KeyCode.LeftShift;

    [Header("Ground Check")]
    public float height;
    public LayerMask ground;
    bool grounded;

    public Transform orient;

    float horizontalInput;
    float verticalInput;

    Vector3 mvDir;
    Rigidbody rb;

    public MvmtState state;
    public enum MvmtState
    {
        walking,
        sprinting,
        inAir
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        canJump = true;
    }

    private void Update()
    {
        //check ground with downward raycast
        grounded = Physics.Raycast(transform.position, Vector3.down, height * 0.5f + 0.2f, ground);

        GetInput();
        ControlSpeed();
        StateHandling();

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

        if (Input.GetKey(jumpBind) && canJump && grounded)
        {
            canJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCD);
        }
    }

    private void StateHandling()
    {
        //when sprinting
        if (grounded && Input.GetKey(sprintBind))
        {
            state = MvmtState.sprinting;
            mvSpd = sprintSpd;
        } else if (grounded)
        {
            state = MvmtState.walking;
            mvSpd = walkSpd;
        } else
        {
            state = MvmtState.inAir;
        }
    }

    private void Movement()
    {
        mvDir = orient.forward * verticalInput + orient.right * horizontalInput;

        if (grounded)
            rb.AddForce(mvDir * mvSpd * 10f, ForceMode.Force);
        else if (!grounded)
            rb.AddForce(mvDir * mvSpd * 10f * airMulti, ForceMode.Force);


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

    private void Jump()
    {
        //reset y vel
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    public void ResetJump()
    {
        canJump = true;
    }
}
