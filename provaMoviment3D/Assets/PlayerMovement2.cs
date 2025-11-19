using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement2 : MonoBehaviour, InputSystem_Actions.IPlayerMovementActions
{
    private float MoveSpeed = 7;

    public Transform Orientation;
    public Vector3 MoveDirection;
    public Rigidbody rb;

   
    private float playerHeight = 2;
    [SerializeField] private LayerMask whatIsGround;
    private bool grounded;
    private float grondDrag = 5;
    private float jumpForce = 12;
    private float jumpCooldown = 0.25f;
    private float airMultipliyer = 0.4f;
    private bool readyToJump = true;
    private InputSystem_Actions inputActions;


    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        inputActions.PlayerMovement.SetCallbacks(this);
        rb = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        inputActions.Enable();
    }
    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        
        Debug.Log(grounded);

        if (grounded)
            rb.linearDamping = grondDrag;
        else
            rb.angularDamping = 0;

        SpeedControll();

       
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

    public void OnJump(InputAction.CallbackContext context)
    {
        Debug.Log("salto");
        if (context.performed) 
        {
            Debug.Log("salto context");
            if (readyToJump && grounded)
            {
                Debug.Log("he saltat");
                readyToJump = false;
                Jump();
                Invoke(nameof(ResetJump), jumpCooldown);
            }
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


    public void OnMove(InputAction.CallbackContext context)
    {
        Debug.Log("em moc");
        Vector2 direction = context.ReadValue<Vector2>();
        MoveDirection = Orientation.forward * direction.y + Orientation.right * direction.x;// * direction.z;

        if (grounded)
            rb.AddForce(MoveDirection.normalized * MoveSpeed * 10, ForceMode.Force);
        else if (!grounded)
        {
            rb.AddForce(MoveDirection.normalized * MoveSpeed * 10 * airMultipliyer, ForceMode.Force);

        }
    }
}
