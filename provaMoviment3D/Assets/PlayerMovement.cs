using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour//, InputSystem_Actions.IPlayerActions
{
    PlayerInput playerInput;
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
    }
   

  
}
