using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

[CreateAssetMenu(fileName = "SO_Harvester", menuName = "Harvester", order = 2)]
public class SOHarvester : ScriptableObject
{
    [SerializeField] private float _speed = 0.5f;

    [Header("Increase with time")]
    public float _stepByTime = 1;
    public float _accelerationByTime = 0.1f;
    public float _maxSpeed = 5f;

    public float GetSpeed(float passedTime)
    {
        if (_stepByTime <= 0) return _speed;
        else
        {
            float extraSpeed = (passedTime / _stepByTime) * _accelerationByTime;
            return Mathf.Clamp(_speed + extraSpeed, 0, _maxSpeed);
        }
    }
}