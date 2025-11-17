using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Npc", menuName = "Scriptable Objects/Npc")]
public class NpcSO : ScriptableObject
{
    [SerializeField] private string npcName;

    [SerializeField] public List<TextSO> Dialogues;
}
