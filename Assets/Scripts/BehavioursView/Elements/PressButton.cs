using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PressButton : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private Image ReleasedIcon;
    [SerializeField] private Image PressedIcon;
    public UnityEvent OnPress, OnRelease;
    [Header("Hidden")]
    [SerializeField] private ControlMap.Map _currentMap;

    public Action OnButtonDown, OnButtonUp;
    private bool _completed, _active;

    private void Awake()
    {
        Setup(_currentMap);
    }

    [Button]
    public void Activate(bool value)
    {
        _active = value;
    }

    public void Setup(ControlMap.Map map)
    {
        _currentMap = map;

        ReleasedIcon.sprite = map.ReleaseIcon;
        PressedIcon.sprite = map.PressedIcon;

        ReleasedIcon.enabled = true;
        PressedIcon.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKey(_currentMap.KeyCode) && _active)
        {
            if (!PressedIcon.enabled)
            {
                ReleasedIcon.enabled = false;
                PressedIcon.enabled = true;
            }
            if (Input.GetKeyDown(_currentMap.KeyCode))
            {
                OnPress?.Invoke();
                OnButtonDown?.Invoke();
            }
        }
        else
        {
            if (!ReleasedIcon.enabled)
            {
                ReleasedIcon.enabled = true;
                PressedIcon.enabled = false;
            }
            if (Input.GetKeyUp(_currentMap.KeyCode))
            {
                OnRelease?.Invoke();
                OnButtonUp?.Invoke();
            }
        }
    }

}
