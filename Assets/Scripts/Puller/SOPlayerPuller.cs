using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_PlayerPuller", menuName = "Pullers/PlayerPuller", order = 0)]
public class SOPlayerPuller : SOPuller
{
    public ControlMap ControlMap => _controlMap;
    [SerializeField] private ControlMap _controlMap;
}

[System.Serializable]
public class ControlMap
{
    public List<Map> KeysMapped = new List<Map>();

    public Action<Direction> OnKeyDown;

    [System.Serializable]
    public struct Map
    {
        public KeyCode KeyCode;
        public Direction Direction;

    }

    public void CheckKeyDown()
    {
        for (int i = 0; i < KeysMapped.Count; i++)
        {
            if (Input.GetKeyDown(KeysMapped[i].KeyCode))
            {
                OnKeyDown?.Invoke(KeysMapped[i].Direction);
            }
        }
    }
}