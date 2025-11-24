using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement2 : MonoBehaviour, InputSystem_Actions.IPlayerMovementActions
{
    

    public Transform Orientation;
    public Vector3 MoveDirection;
    public Rigidbody rb;

    private float playerHeight = 2;
    [SerializeField] private LayerMask whatIsGround;
    private bool grounded;
    private float groundDrag = 5;
    private float jumpForce = 8;
    private float jumpCooldown = 0.25f;
    private float airMultiplier = 0.4f;
    private bool readyToJump = true;
    private bool isSprinting = false;
    [SerializeField] private LayerMask interactLayer;
    private InputSystem_Actions inputActions;
    [SerializeField] private Transform seePoint;
    private IInteractuable lastInteractuable = null;
    private IInteractuable interacting = null;
    private float MoveSpeed = 7;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        inputActions.PlayerMovement.SetCallbacks(this);
        rb = GetComponent<Rigidbody>();
        

        Cursor.visible = false;
        
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
        Debug.DrawRay(transform.position, Vector3.down, grounded ? Color.green : Color.red);
        //Debug.Log(grounded);
        if (grounded)
        {
            rb.linearDamping = groundDrag;
            rb.AddForce(MoveDirection.normalized * MoveSpeed * (isSprinting ? 15 : 10), ForceMode.Force);
        }
        else
        {
            rb.linearDamping = 3f;
            rb.AddForce(MoveDirection.normalized * MoveSpeed * (isSprinting ? 15 : 10) * airMultiplier, ForceMode.Force);
        }
        SpeedControll();

        Vector3 lookDir = seePoint.forward;

        Ray ray = new Ray(seePoint.position, lookDir);
        
        RaycastHit hit;
       
        Debug.DrawRay(seePoint.position, lookDir * 3f, Color.yellow);

        bool hitBool = Physics.Raycast(ray, out hit, 3f,interactLayer);
        if (hitBool)
        {
            interacting = hit.collider.GetComponent<IInteractuable>();
            if (interacting != null)
            {
                interacting.SetActiveDiaolgue(true);
                if(interacting != lastInteractuable &&  lastInteractuable != null)
                {
                    lastInteractuable.SetActiveDiaolgue(false);
                }
                lastInteractuable = interacting;
            }
        }
        else if( !hitBool && lastInteractuable != null)
        {
            //Debug.Log("fuera");
            lastInteractuable.SetActiveDiaolgue(false);
            lastInteractuable.TurnOff();
            lastInteractuable = null;
           
            interacting.SetActiveDiaolgue(false);
            interacting = null;
        }
      
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        
        Vector2 direction = context.ReadValue<Vector2>();
        MoveDirection = Orientation.forward * direction.y + Orientation.right * direction.x;

    }
    private void SpeedControll()
    {
        Vector3 flatVel = rb.linearVelocity;

        if (flatVel.magnitude > MoveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * MoveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }

    }

    public void OnJump(InputAction.CallbackContext context)
    {
       
       
        if (context.performed) 
        {
     

            if (readyToJump && grounded)
            {
              

                readyToJump = false;
                Jump();
                Invoke(nameof(ResetJump), jumpCooldown);
            }
        }
    }
    public void Jump()
    {
       
       
        rb.linearVelocity = Vector3.up * jumpForce;
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (grounded)
            {
                
                isSprinting = true;
              
            }
        }
        else if (context.canceled)
        {
            isSprinting = false;
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
        {


            if (interacting != null)
            {
                Debug.Log("hola");
            }
        }
        if (context.canceled)
            Debug.Log("canceled");
        if (context.performed)
            Debug.Log("Perf");
    }
}
