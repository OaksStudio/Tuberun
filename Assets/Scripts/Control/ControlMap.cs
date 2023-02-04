using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum InputActions { UP = 0, DOWN = 1, RIGHT = 2, LEFT = 3, ANY = 4, NONE = 5, CONFIRM = 6, CANCEL = 7 }

[System.Serializable]
public class ControlMap
{
    public List<Map> KeysMapped = new List<Map>();

    public Action<InputActions> OnKeyDown;

    [System.Serializable]
    public struct Map
    {
        public KeyCode KeyCode;
        [FormerlySerializedAs("Direction")] public InputActions InputAction;
        public Sprite ReleaseIcon;
        public Sprite PressedIcon;
    }

    public void CheckKeyDown(List<InputActions> actionsFilter)
    {
        for (int i = 0; i < KeysMapped.Count; i++)
        {
            if (!actionsFilter.Contains(KeysMapped[i].InputAction)) continue;
            if (Input.GetKeyDown(KeysMapped[i].KeyCode))
            {
                OnKeyDown?.Invoke(KeysMapped[i].InputAction);
            }
        }
    }

    public void CheckKeyDown()
    {
        for (int i = 0; i < KeysMapped.Count; i++)
        {
            if (Input.GetKeyDown(KeysMapped[i].KeyCode))
            {
                OnKeyDown?.Invoke(KeysMapped[i].InputAction);
            }
        }
    }
}