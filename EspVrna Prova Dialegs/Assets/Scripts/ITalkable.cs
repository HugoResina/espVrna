using UnityEngine;
using System.Collections.Generic;

public interface ITalkable
{
    List<TextSO> Dialogue { get; set; }
    int Index { get; set; }

    void ShowText(int i);
}
