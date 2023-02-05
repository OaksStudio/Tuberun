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
    public UnityEvent OnPulledEvent, OnReleasedEvent, OnMissEvent, OnStopEvent, OnResetEvent, OnDieEvent;

    [Header("Model")]
    public Animator animator;
    public Transform Model;
    public Transform HoleMask;
    public SpriteRenderer MainBody;

    [Header("Pulling Setup")]
    private const float PullingSpeed = 5f;
    public Transform DeeperPosition;
    public Transform SurfacePosition;

    [Header("Animations")]
    [Range(0, 1)] public float AlmostLeavingPoint = 0.8f;
    public string PullTrigger = "OnPull";
    public string MissTrigger = "OnMiss";
    public string LeaveTrigger = "OnLeave";
    public string DieTrigger = "OnDie";
    public string AlmostLeavingBool = "AlmostLeaving";
    public string RandomFloat = "Random";

    private Tuber _tuber;

    private void Awake()
    {
        _tuber = GetComponent<Tuber>();
        if (!animator) animator = GetComponentInChildren<Animator>();

        _tuber.OnPull += PulledFeedback;
        _tuber.OnRelease += ReleasedFeedback;
        _tuber.OnMiss += MissedFeedback;
        _tuber.OnStop += StopFeedback;
        _tuber.OnSetup += Initialize;
        _tuber.OnKill += Kill;
    }

    private void Initialize()
    {
        if (!_tuber.TuberInfo) return;
        //MainBody.color = _tuber.TuberInfo.BodyColor;
        Model.transform.position = DeeperPosition.position;
    }

    [Button]
    private void PulledFeedback()
    {
        animator.SetFloat(RandomFloat, UnityEngine.Random.Range(0f, 1f));
        PullPosition();
        OnPulledEvent?.Invoke();
        animator.SetTrigger(PullTrigger);
    }

    [Button]
    private void PullPosition()
    {
        float progress = Mathf.InverseLerp(_tuber.MaxDeepness, 0, _tuber.CurrentDeepness);
        Vector2 target = Vector2.Lerp(DeeperPosition.position, SurfacePosition.position, progress);
        Model.transform.position = Vector2.MoveTowards(Model.transform.position, target, PullingSpeed * Time.deltaTime);

        if (progress >= AlmostLeavingPoint)
        {
            if (!animator.GetBool(AlmostLeavingBool))
                animator.SetBool(AlmostLeavingBool, true);
        }
        else
        {
            if (animator.GetBool(AlmostLeavingBool))
                animator.SetBool(AlmostLeavingBool, false);
        }
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
        animator.SetTrigger(MissTrigger);
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

    private void Kill()
    {
        animator.SetTrigger(DieTrigger);
        OnDieEvent?.Invoke();
    }

    public void StartRunning()
    {
        _tuber.MoveToPosition.StartMoving = true;
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
