using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    PlayerInput playerInput;
    InputAction lookAction;
    public Transform orientation;
    public float SensX;
    public float SensY;
    float YRotation;
    float XRotation;
    public GameObject seePoint;
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        lookAction = playerInput.actions.FindAction("Look");
        
        Cursor.visible = false;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        Cursor.lockState = CursorLockMode.Locked;


    }
    private void Update()
    {
        float mouseX = lookAction.ReadValue<Vector2>().x * Time.deltaTime * SensX;
        float mouseY = lookAction.ReadValue<Vector2>().y * Time.deltaTime * SensY;

        YRotation += mouseX;
        XRotation -= mouseY;
        XRotation = Mathf.Clamp(XRotation, -90f, 90f);
        Look();
    }
    void Look()
    {

        transform.rotation = Quaternion.Euler(XRotation, YRotation, 0);
        orientation.rotation = Quaternion.Euler(0, YRotation, 0);
        seePoint.transform.rotation = transform.rotation;

    }
}
