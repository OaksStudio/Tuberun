using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEvents : MonoBehaviour
{
    public UnityEvent OnEvent1, OnEvent2, OnEvent3;

    public void Event1()
    {
        OnEvent1?.Invoke();
    }

    public void Event2()
    {
        OnEvent2?.Invoke();
    }

    public void Event3()
    {
        OnEvent3?.Invoke();
    }
}
