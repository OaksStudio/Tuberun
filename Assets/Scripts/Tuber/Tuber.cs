using System;
using System.Collections;
using System.Collections.Generic;
using Jozi.Pools;
using UnityEngine;

public class Tuber : MonoBehaviour
{
    public int RowID => _rowID;
    private int _rowID;

    [SerializeField] private List<Direction> _directions = new List<Direction>();

    [SerializeField] private float CurrentDeepness => _currentDeepness;
    [SerializeField] private float _currentDeepness;

    [SerializeField] private float MaxDeepness => _maxDeepness;
    [SerializeField] private float _maxDeepness;

    [SerializeField] private int _nextDirectionInput;

    private SOTuber _tuberInfo;

    public Action OnPull, OnMiss = delegate { };
    public Action<Tuber> OnRelease = delegate { };

    public void Setup(int rowID, SOTuber tuberInfo)
    {
        _rowID = rowID;
        _currentDeepness = _maxDeepness = tuberInfo.Deepness;
        _directions = tuberInfo.directions;
        _nextDirectionInput = 0;
    }

    public void TryPull(Direction direction, float pullForce)
    {
        Direction currentDir = _directions[_nextDirectionInput];

        if (currentDir == Direction.ANY || currentDir == direction)
        {
            Pull(pullForce);
            Debug.Log($"Pulled!");

            _nextDirectionInput++;
            if (_nextDirectionInput >= _directions.Count)
            {
                _nextDirectionInput = 0;
            }
        }
        else
        {
            OnMiss?.Invoke();
            Debug.Log($"Missed!");
        }
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
        Debug.Log($"Released!");
        OnRelease?.Invoke(this);
        PoolCommand.ReleaseObject(gameObject);
    }

}
