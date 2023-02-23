using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.InputSystem;

public enum InputActions { UP = 0, DOWN = 1, RIGHT = 2, LEFT = 3, ANY = 4, NONE = 5, CONFIRM = 6, CANCEL = 7, RETURN = 8 }

[System.Serializable]
public class ControlMap
{
    public PlayerInput PlayerInput;
    public InputActionAsset Asset;
    public List<Map> KeysMapped = new List<Map>();

    public Action<InputActions> OnKeyDown;

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
        // for (int i = 0; i < KeysMapped.Count; i++)
        // {
        //     if (Input.GetKeyDown(KeysMapped[i].KeyCode))
        //     {
        //         OnKeyDown?.Invoke(KeysMapped[i].InputAction);
        //        // Debug.Log(KeysMapped[i].InputAction.ToString());
        //     }
        // }
    }

    public void Setup(PlayerInput playerInput)
    {
        if (!playerInput) return;
        PlayerInput = playerInput;
        PlayerInput.onActionTriggered += ReadAction;

        KeysMapped.ForEach(key => key.Action = PlayerInput.
        currentActionMap.FindAction(key.InputAction.ToString()));
    }

    public void ReleaseSetup()
    {
        if (!PlayerInput) return;
        PlayerInput.onActionTriggered -= ReadAction;
    }

    private void ReadAction(InputAction.CallbackContext context)
    {
        for (int i = 0; i < KeysMapped.Count; i++)
        {
            if (KeysMapped[i].Action.Equals(context.action) && context.performed)
            {
                OnKeyDown?.Invoke(KeysMapped[i].InputAction);
                Debug.Log($"{Asset.name} {KeysMapped[i].InputAction.ToString()} {context.phase} {context.ReadValue<float>()}");
            }
        }
    }
}

[System.Serializable]
public class Map
{
    public KeyCode KeyCode;
    public InputAction Action;
    [FormerlySerializedAs("Direction")] public InputActions InputAction;
    public Sprite ReleaseIcon;
    public Sprite PressedIcon;
}