using UnityEngine;

[CreateAssetMenu(fileName = "Answer", menuName = "Scriptable Objects/Text-Answer")]
public class AnswerSO : ScriptableObject
{
    [SerializeField] public string text;
    [SerializeField] public TextSO nextText;
}
