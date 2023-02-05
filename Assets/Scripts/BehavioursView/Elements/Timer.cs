using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI Text;

    public void SetTime(int minutes, int seconds)
    {
        Text.text = $"{(minutes).ToString("00")}:{(seconds).ToString("00")}";
    }
}
