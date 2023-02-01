using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tuber : MonoBehaviour
{
    public SOTuber TuberInfo;

    [SerializeField] private List<Direction> _directions = new List<Direction>();

    [SerializeField] private int CurrentDeepness => _currentDeepness;
    [SerializeField] private int _currentDeepness;
    [SerializeField] private int _nextDirectionInput;

    public ControlMap controlMap;

    public Action OnPull, OnMiss, OnRelease = delegate { };

    private void Start()
    {
        Setup(TuberInfo);

        controlMap.OnKeyDown += CheckDirection;
    }

    private void OnDestroy()
    {
        controlMap.OnKeyDown -= CheckDirection;
    }

    private void Update()
    {
        controlMap.CheckKeyDown();
    }

    public void Setup(SOTuber tuberInfo)
    {
        _currentDeepness = TuberInfo.Deepness;
        _directions = tuberInfo.directions;
        _nextDirectionInput = 0;
    }

    private void CheckDirection(Direction direction)
    {
        Direction currentDir = _directions[_nextDirectionInput];

        if (currentDir == Direction.ANY || currentDir == direction)
        {
            Pull(1);
            Debug.Log($"Try Pull!");

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

    public void Pull(int pullValue)
    {
        if (_currentDeepness <= 0) return;

        _currentDeepness -= pullValue;

        OnPull?.Invoke();

        if (_currentDeepness <= 0)
        {
            Release();
        }
    }

    public void Release()
    {
        Debug.Log($"Released!");
        OnRelease?.Invoke();
    }

}
