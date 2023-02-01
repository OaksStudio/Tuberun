using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_TuberInfo", menuName = "Tuber")]
public class SOTuberInfo : ScriptableObject
{
    public int Deepness = 10;

    [Header("Input Sequence")]
    public List<Direction> directions = new List<Direction>() { Direction.ANY };

}


public enum Direction { UP = 0, DOWN = 1, RIGHT = 2, LEFT = 3, ANY = 4, NONE = 5 }
