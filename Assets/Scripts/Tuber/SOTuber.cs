using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_TuberInfo", menuName = "Tuber")]
public class SOTuber : ScriptableObject
{
    public int Deepness = 10;

    [Header("Input Sequence")]
    public List<Direction> directions = new List<Direction>() { Direction.ANY };

}

