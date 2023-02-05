using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CallConfirm : MonoBehaviour
{
    public UnityEvent OnYes;
    public ConfirmBehaviour ConfirmBehaviour;
    
    public void Call()
    {
        ConfirmBehaviour.SetupConfirm(YesEvent);
    }

    private void YesEvent()
    {
        OnYes?.Invoke();
    }
}
