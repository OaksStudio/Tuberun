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
    [ReadOnly, SerializeField] private List<InputActions> _directions = new List<InputActions>();
    [ReadOnly, SerializeField] private float _currentDeepness;
    [ReadOnly, SerializeField] private float _maxDeepness;
    [ReadOnly, SerializeField] private int _nextDirectionInput;
    [ReadOnly, SerializeField] private SOTuber _tuberInfo;

    [Header("Move")]
    public MoveToPosition MoveToPosition;
    public float DistanceToTarget = 2;

    public Action OnPull, OnMiss, OnStop, OnSetup, OnKill = delegate { };
    public Action<Tuber> OnRelease = delegate { };

    private BoxCollider2D _boxCollider;
    private bool _stopped = false;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    [Button]
    public void Setup(int rowID, SOTuber tuberInfo)
    {
        _rowID = rowID;
        _tuberInfo = tuberInfo;
        _currentDeepness = _maxDeepness = tuberInfo.Deepness;
        _directions = tuberInfo.directions;
        _nextDirectionInput = 0;
        _boxCollider.enabled = true;
        _stopped = false;
        MoveToPosition.TargetPosition = GameManager.Instance.GameMode.TubersTarget[rowID];
        MoveToPosition.StartMoving = false;
        OnSetup?.Invoke();
    }

    private void Update()
    {
        if (MoveToPosition.StartMoving)
        {
            
        }
    }

    public void TryPull(InputActions direction, float pullForce)
    {
        if (direction == InputActions.NONE) return;

        InputActions currentDir = _directions[_nextDirectionInput];

        if (currentDir == InputActions.ANY || currentDir == direction)
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

    public InputActions GetCorrectDirection()
    {
        return _directions[_nextDirectionInput];
    }

    public void StopTuber()
    {
        if (_stopped) return;
        _stopped = true;
        OnStop?.Invoke();
    }

    public void KillTuber()
    {
        OnKill?.Invoke();
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
            GameManager.Instance.AddPoint(_tuberInfo.PointsByPull, _rowID);
            OnPull?.Invoke();
        }
    }

    public void Release()
    {
        GameManager.Instance.AddPoint(_tuberInfo.PointsByRelease, _rowID);
        _boxCollider.enabled = false;
        OnRelease?.Invoke(this);
    }

    public void Reset()
    {
        PoolCommand.ReleaseObject(gameObject);
    }

}
