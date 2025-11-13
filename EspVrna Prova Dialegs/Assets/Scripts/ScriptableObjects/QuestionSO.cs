using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Question", menuName = "Scriptable Objects/Text-Question")]
public class QuestionSO : IndexableTextSO
{
    private List<AnswerSO> answers;
}
