using UnityEngine;
using UnityEngine.UI;

public class MockNPC : MonoBehaviour, IInteractuable
{
    public bool isInteracting;
    public string mesage;

    public Text _text;

    [SerializeField] private GameObject panel;


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
        panel.active = state;
 
        
    }

  
    
    public void TurnOff()
    {
        _text.text = "";
        //disable canvas?
    }

}
