using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Linq;

public class PressButton : MonoBehaviour
{
    public PlayerInput PlayerInput => _playerInput;
    [Header("Setup")]
    [SerializeField] private Image ReleasedIcon;
    [SerializeField] private Image PressedIcon;
    public UnityEvent OnPress, OnRelease;
    [Header("Hidden")]
    [SerializeField] private Map _currentMap;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private InputActionAsset _asset;

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

    public void Setup(Map map, InputActionAsset asset = null)
    {
        _currentMap = map;

        ReleasedIcon.sprite = map.ReleaseIcon;
        PressedIcon.sprite = map.PressedIcon;

        ReleasedIcon.enabled = true;
        PressedIcon.enabled = false;

        if (asset != null)
            _asset = asset;

        if (!_asset) return;

        if (_playerInput)
            _playerInput.onActionTriggered -= ReadAction;

        _playerInput = FindObjectsOfType<PlayerInput>().
        ToList().Find(p => p.actions == _asset);

        if (!_playerInput) return;
        _playerInput.onActionTriggered += ReadAction;

        _currentMap.Action = _playerInput.currentActionMap.
        FindAction(_currentMap.InputAction.ToString());
    }

    private void ReadAction(InputAction.CallbackContext context)
    {
        if (!_currentMap.Action.Equals(context.action)) return;

        if (context.performed && _active)
        {
            if (!PressedIcon.enabled)
            {
                ReleasedIcon.enabled = false;
                PressedIcon.enabled = true;
            }
            OnPress?.Invoke();
            OnButtonDown?.Invoke();
        }
        else
        {
            if (!ReleasedIcon.enabled)
            {
                ReleasedIcon.enabled = true;
                PressedIcon.enabled = false;
            }

            if (context.canceled)
            {
                OnRelease?.Invoke();
                OnButtonUp?.Invoke();
            }
        }
        //Debug.Log($" {_currentMap.InputAction.ToString()} {context.phase} {context.ReadValue<float>()}");
    }

    private void OnDestroy()
    {
        if (_playerInput)
            _playerInput.onActionTriggered -= ReadAction;
    }

}
