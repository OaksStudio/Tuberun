using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_TuberInfo", menuName = "Tuber")]
public class SOTuber : ScriptableObject
{
    public int Deepness = 10;
    public int PointsByPull = 10;
    public int PointsByRelease = 50;

    public Color BodyColor = Color.white;

    [Header("Input Sequence")]
    public List<InputActions> directions = new List<InputActions>() { InputActions.ANY };
    public Tuber TuberPrefab;

}

