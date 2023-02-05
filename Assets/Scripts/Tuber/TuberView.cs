using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Tuber))]
public class TuberView : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent OnPulledEvent, OnReleasedEvent, OnMissEvent, OnStopEvent, OnResetEvent;

    [Header("Model")]
    public Transform Model;
    public Transform HoleMask;
    public SpriteRenderer MainBody;

    [Header("Pulling Setup")]
    private const float PullingSpeed = 5f;
    public Transform DeeperPosition;
    public Transform SurfacePosition;

    private Tuber _tuber;

    private void Awake()
    {
        _tuber = GetComponent<Tuber>();

        _tuber.OnPull += PulledFeedback;
        _tuber.OnRelease += ReleasedFeedback;
        _tuber.OnMiss += MissedFeedback;
        _tuber.OnStop += StopFeedback;
        _tuber.OnSetup += Initialize;
    }

    private void Initialize()
    {
        if (!_tuber.TuberInfo) return;
        //MainBody.color = _tuber.TuberInfo.BodyColor;
    }

    [Button]
    private void PulledFeedback()
    {
        PullPosition();
        OnPulledEvent?.Invoke();
    }

    [Button]
    private void PullPosition()
    {
        Vector2 target = Vector2.Lerp(DeeperPosition.position, SurfacePosition.position, Mathf.InverseLerp(_tuber.MaxDeepness, 0, _tuber.CurrentDeepness));
        Model.transform.position = Vector2.MoveTowards(Model.transform.position, target, PullingSpeed * Time.deltaTime);
    }

    [Button]
    private void ReleasedFeedback(Tuber tuber)
    {
        OnReleasedEvent?.Invoke();
    }

    [Button]
    private void MissedFeedback()
    {
        OnMissEvent?.Invoke();
    }

    [Button]
    private void StopFeedback()
    {
        OnStopEvent?.Invoke();
    }

    [Button]
    private void OnDisable()
    {
        Reset();
    }

    [Button]
    private void SetSurface()
    {
        Model.transform.position = SurfacePosition.position;
    }

    [Button]
    private void Reset()
    {
        Model.transform.position = DeeperPosition.position;
        OnResetEvent?.Invoke();
    }

    private void OnDestroy()
    {
        _tuber.OnPull -= PulledFeedback;
        _tuber.OnRelease -= ReleasedFeedback;
        _tuber.OnMiss -= MissedFeedback;
    }
}
