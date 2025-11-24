using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

public interface IInteractuable 
{
    public void SetActiveDiaolgue(bool state);
    public void TurnOff();

}
