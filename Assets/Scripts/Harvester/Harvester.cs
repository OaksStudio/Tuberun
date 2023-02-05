using System;
using System.Collections;
using System.Collections.Generic;
using Jozi.Utilities.Primitives;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class Harvester : MonoBehaviour
{
    [Header("Setup")]
    public SOString TuberTag;
    public float MoveTowardsSpeed = 1;

    public float CurrentSpeed => _currentSpeed;

    public Action<int> OnHarvest;
    public Action OnStop;

    public UnityEvent OnSetupEvent, OnStopEvent, OnHarvestEvent;


    private SOHarvester _harvester;
    private Rigidbody2D _rigidbody;
    [SerializeField] private float _currentSpeed = 0;
    private float _passedTime;
    private bool _stopped;

    [Button]
    public void Setup(SOHarvester harvester)
    {
        _harvester = harvester;
        _passedTime = Time.time;
        _stopped = false;
        OnSetupEvent?.Invoke();
    }

    public void StopHarvester()
    {
        if (_stopped) return;
        _stopped = true;
        _currentSpeed = 0;
        OnStop?.Invoke();
        OnStopEvent?.Invoke();
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (_stopped || !_harvester) return;
        _currentSpeed = Mathf.MoveTowards(_currentSpeed, _harvester.GetSpeed(Time.time - _passedTime), MoveTowardsSpeed * Time.deltaTime);
        _rigidbody.MovePosition(transform.position + Vector3.right * _currentSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != TuberTag.Value || _stopped) return;
        OnHarvest?.Invoke(other.gameObject.GetComponent<Tuber>().RowID);
        OnHarvestEvent?.Invoke();
    }
}
