using System;
using UnityEngine;
using UnityEngine.UI;

public class MockNPC : MonoBehaviour, IInteractuable
{
    public bool isInteracting;
    public string mesage;

    public Text _text;

    [SerializeField] private GameObject panel;
    public static event Action<bool> interactingFlag;


    private void Update()
    {
        if (isInteracting)
        {

            if (_text != null)
            {
                _text.text = mesage;
            }
        }

    }
  
    public void SetActiveDiaolgue(bool state)
    {
        isInteracting = state;
        panel.SetActive(state);
        Cursor.visible = state;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        Cursor.lockState = state? CursorLockMode.None : CursorLockMode.Locked;

        

        interactingFlag?.Invoke(state);
        Debug.Log("setactivedialogue ----->" + state );
        if (!state)
        {
            interactingFlag?.Invoke(false);

        }

    }

  
    
    public void TurnOff()
    {
        _text.text = "";
        //disable canvas?
    }

}
