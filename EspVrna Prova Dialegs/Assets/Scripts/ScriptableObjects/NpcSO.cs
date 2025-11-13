using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Npc", menuName = "Scriptable Objects/Npc")]
public class NpcSO : ScriptableObject, ITalkable
{
    public List<TextSO> Dialogue { get; set; }
    public int Index { get; set; }

    public void ShowText(int i)
    {
        if (i < Dialogue.Count)
        {
            if (Dialogue[i].GetType() == typeof(QuestionSO))
            {
                Debug.Log("huaeruhgiewrahgoiaerhgiearh");
            }
        }
        else
        {
            Debug.Log("No more dialogue available.");
        }
    }
}
