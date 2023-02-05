using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SO_BotPuller", menuName = "Pullers/BotPuller", order = 0)]
public class SOBotPuller : SOPuller
{
    public Vector2 CooldownRange = new Vector2(0.1f, 0.2f);
    public float DelayToStart = 2f;
    [Range(0f, 100f)]
    public float Accuracy = 50f;
    public float MaxAccuracy => _maxAccuracy;
    private const float _maxAccuracy = 100;
}
