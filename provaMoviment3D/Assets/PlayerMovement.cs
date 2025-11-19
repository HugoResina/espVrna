using NUnit.Framework.Interfaces;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour//, InputSystem_Actions.IPlayerActions
{


    private float MoveSpeed = 7;

    public Transform Orientation;
    public Vector3 MoveDirection;
    public Rigidbody rb;
    PlayerInput playerInput;
    InputAction moveAction;
    InputAction jumpAction;
    private float playerHeight;
    [SerializeField]private LayerMask whatIsGround;
    private bool grounded;
    private float grondDrag = 5;
    private float jumpForce = 12;
    private float jumpCooldown = 0.25f;
    private float airMultipliyer = 0.4f;
    private bool readyToJump;

    private void Start()
    {


        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        playerInput = GetComponent<PlayerInput>();


        moveAction = playerInput.actions.FindAction("Move");
        jumpAction = playerInput.actions.FindAction("Jump");

        jumpAction.AddBinding("<Keyboard>/Space");
        jumpAction.Enable();

    }
    private void Update()
    {

        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        
        if (grounded)
            rb.linearDamping = grondDrag;
        else 
            rb.angularDamping = 0;

        SpeedControll();
        
        if(readyToJump && grounded && jumpAction.triggered)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    public void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void SpeedControll()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

        if (flatVel.magnitude > MoveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * MoveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }

    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        //Debug.Log(moveAction.ReadValue<Vector2>());
        Vector3 direction = moveAction.ReadValue<Vector2>(); 
        MoveDirection = Orientation.forward * direction.y + Orientation.right * direction.x * direction.z;

        if(grounded)
            rb.AddForce(MoveDirection.normalized * MoveSpeed * 10, ForceMode.Force);
        else if (!grounded)
        {
            rb.AddForce(MoveDirection.normalized * MoveSpeed * 10 * airMultipliyer, ForceMode.Force);

        }

    }

    /*PlayerInput playerInput;
    InputAction moveAction;
    


    [SerializeField] private float Speed = 10f;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        
        moveAction = playerInput.actions.FindAction("Move");
        
    }

   
    void Update()
    {
        MovePlayer();
        
    }
    void MovePlayer()
    {
        Debug.Log(moveAction.ReadValue<Vector2>());
        Vector2 direction = moveAction.ReadValue<Vector2>();
        transform.position += new Vector3(direction.x, 0, direction.y) * Time.deltaTime * Speed;    

    }*/



}
