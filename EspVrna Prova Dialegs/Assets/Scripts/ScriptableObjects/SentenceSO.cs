using UnityEngine;

[CreateAssetMenu(fileName = "Sentence", menuName = "Scriptable Objects/Text-Sentence")]
public class SentenceSO : TextSO
{
    [SerializeField] public TextSO nextText;
}
