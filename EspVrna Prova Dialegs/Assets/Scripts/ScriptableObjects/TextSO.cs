using UnityEngine;

//[CreateAssetMenu(fileName = "Text", menuName = "Scriptable Objects/Text")]
public abstract class TextSO : ScriptableObject
{
    [Header("Text Data")]
    [Tooltip("Unique identifier for this text")]
    [SerializeField] public string Id;

    [TextArea(3, 10)]
    [Tooltip("The text content")]
    [SerializeField] public string Text;
}
