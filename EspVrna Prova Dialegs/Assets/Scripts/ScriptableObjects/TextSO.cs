using UnityEngine;

//[CreateAssetMenu(fileName = "Text", menuName = "Scriptable Objects/Text")]
public abstract class TextSO : ScriptableObject
{
    [TextArea(3, 10)]
    [SerializeField] public string text;
}
