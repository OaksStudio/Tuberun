using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class HoldButton : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private Image ReleasedIcon;
    [SerializeField] private Image PressedIcon;
    [SerializeField] private List<Image> ReleasedFillers = new List<Image>();
    [SerializeField] private List<Image> PressedFillers = new List<Image>();

    [Header("Config")]
    [SerializeField] private float _addSpeed = 1;
    [SerializeField] private float _removeSpeed = 2;

    [SerializeField] private bool _stopOncompleted;

    [Header("Hidden")]
    [SerializeField] private ControlMap.Map _currentMap;
    [SerializeField, ReadOnly] private float _currentProgress;

    public Action OnComplete;

    private bool _completed;

    private void Awake()
    {
        Setup(_currentMap);
    }

    public void Setup(ControlMap.Map map)
    {
        _currentMap = map;

        ReleasedIcon.sprite = map.ReleaseIcon;
        PressedIcon.sprite = map.PressedIcon;
        ReleasedFillers.ForEach(f => f.sprite = map.ReleaseIcon);
        PressedFillers.ForEach(f => f.sprite = map.PressedIcon);

        ReleasedIcon.enabled = true;
        ReleasedFillers.ForEach(f => f.enabled = true);

        PressedIcon.enabled = false;
        PressedFillers.ForEach(f => f.enabled = false);

        Reset();
    }

    public void Reset()
    {
        _completed = false;
    }

    private void Update()
    {
        if (_completed && _stopOncompleted) return;

        if (Input.GetKey(_currentMap.KeyCode))
        {
            _currentProgress = Mathf.Clamp01(_currentProgress + _addSpeed * Time.deltaTime);

            if (!PressedIcon.enabled)
            {
                ReleasedIcon.enabled = false;
                ReleasedFillers.ForEach(f => f.enabled = false);

                PressedIcon.enabled = true;
                PressedFillers.ForEach(f => f.enabled = true);
            }
        }
        else
        {
            _currentProgress = Mathf.Clamp01(_currentProgress - _removeSpeed * Time.deltaTime);
            if (!ReleasedIcon.enabled)
            {
                ReleasedIcon.enabled = true;
                ReleasedFillers.ForEach(f => f.enabled = true);

                PressedIcon.enabled = false;
                PressedFillers.ForEach(f => f.enabled = false);
            }
        }

        Progress(_currentProgress);

        if (_currentProgress >= 1)
        {
            _completed = true;
            OnComplete?.Invoke();
        }
    }

    private void Progress(float value)
    {
        ReleasedFillers.ForEach(f => f.fillAmount = value);
        PressedFillers.ForEach(f => f.fillAmount = value);
    }
}
