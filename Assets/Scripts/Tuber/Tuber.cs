using System;
using System.Collections;
using System.Collections.Generic;
using Jozi.Pools;
using Sirenix.OdinInspector;
using UnityEngine;

public class Tuber : MonoBehaviour
{
    public int RowID => _rowID;
    public SOTuber TuberInfo => _tuberInfo;
    public float CurrentDeepness => _currentDeepness;
    public float MaxDeepness => _maxDeepness;

    [ReadOnly, SerializeField] private int _rowID;
    [ReadOnly, SerializeField] private List<Direction> _directions = new List<Direction>();
    [ReadOnly, SerializeField] private float _currentDeepness;
    [ReadOnly, SerializeField] private float _maxDeepness;
    [ReadOnly, SerializeField] private int _nextDirectionInput;
    [ReadOnly, SerializeField] private SOTuber _tuberInfo;

    public Action OnPull, OnMiss, OnStop, OnSetup = delegate { };
    public Action<Tuber> OnRelease = delegate { };

    private BoxCollider2D _boxCollider;
    private bool _stopped = false;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    public void Setup(int rowID, SOTuber tuberInfo)
    {
        _rowID = rowID;
        _tuberInfo = tuberInfo;
        _currentDeepness = _maxDeepness = tuberInfo.Deepness;
        _directions = tuberInfo.directions;
        _nextDirectionInput = 0;
        _boxCollider.enabled = true;
        _stopped = false;
        OnSetup?.Invoke();
    }

    public void TryPull(Direction direction, float pullForce)
    {
        Direction currentDir = _directions[_nextDirectionInput];

        if (currentDir == Direction.ANY || currentDir == direction)
        {
            Pull(pullForce);

            _nextDirectionInput++;
            if (_nextDirectionInput >= _directions.Count)
            {
                _nextDirectionInput = 0;
            }
        }
        else
        {
            OnMiss?.Invoke();
        }
    }

    public void StopTuber()
    {
        _stopped = true;
        OnStop?.Invoke();
    }

    private void Pull(float pullForce)
    {
        if (_currentDeepness <= 0) return;
        _currentDeepness -= pullForce;

        if (_currentDeepness <= 0)
        {
            Release();
        }
        else
        {
            OnPull?.Invoke();
        }
    }

    public void Release()
    {
        _boxCollider.enabled = false;
        OnRelease?.Invoke(this);
        PoolCommand.ReleaseObject(gameObject);
    }

}
