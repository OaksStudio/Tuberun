using System;
using System.Collections;
using System.Collections.Generic;
using Jozi.Utilities.Primitives;
using UnityEngine;

public class Harvester : MonoBehaviour
{
    [Header("Setup")]
    public SOString TuberTag;
    public float MoveTowardsSpeed = 1;

    public Action<int> OnHarvest;

    private SOHarvester _harvester;
    private Rigidbody2D _rigidbody;
    [SerializeField] private float _currentSpeed = 0;
    private float _passedTime;

    public void Setup(SOHarvester harvester)
    {
        _harvester = harvester;
        _passedTime = Time.time;
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _currentSpeed = Mathf.MoveTowards(_currentSpeed, _harvester.GetSpeed(Time.time - _passedTime), MoveTowardsSpeed * Time.deltaTime);
        _rigidbody.MovePosition(transform.position + Vector3.right * _currentSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != TuberTag.Value) return;
        OnHarvest?.Invoke(other.gameObject.GetComponent<Tuber>().RowID);
    }
}
