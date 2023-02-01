using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SO_BotPuller", menuName = "Pullers/BotPuller", order = 0)]
public class SOBotPuller : SOPuller
{
    [Range(0f, 1f)]
    public float Accuracy = 0.5f;
}
