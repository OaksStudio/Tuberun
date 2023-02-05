using System.Collections;
using System.Collections.Generic;
using Jozi.Utilities.Parameters;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_GameMode", menuName = "GameSettings/GameMode", order = 1)]
public class SOGameMode : ScriptableObject
{
    public string GameModeName = "Normal run";

    public SOScene GameModeScene;

    [Header("Final Visual Result")]
    public bool ShowResultMessage = true;
    public string ResultMessage = "Winner!!!";
    public bool Showtime = true;
    public string RematchText = "Rematch";
}
