using System;
using System.Collections;
using System.Collections.Generic;
using OAKS.Utilities.Views;
using TMPro;
using UnityEngine;

public class HUDBehaviour : ViewBehaviour
{
    public string StartPointText = "";
    public string EndPointText = "";
    public List<TextMeshProUGUI> Points = new List<TextMeshProUGUI>();

    public List<TextMeshProUGUI> CompetitorsNames = new List<TextMeshProUGUI>();

    public List<TextMeshProUGUI> CompetitorsNamesBack = new List<TextMeshProUGUI>();

    public List<Timer> Timers = new List<Timer>();

    [Header("Pause")]
    public ViewBase PauseView;
    public float PauseCooldown = 1;
    private float _timeStamp;

    private void Start()
    {
        GameManager.Instance.OnAddPoint += AddPoint;
        GameManager.Instance.OnSetTime += SetTime;
        foreach (var timer in Timers)
        {
            timer.SetTime(0, 0);
        }

        foreach (var point in Points)
        {
            point.text = $"{StartPointText}{0}{EndPointText}";
        }

        CompetitorsNames[0].text = GameManager.Instance.Competitors[0].PullerName;
        CompetitorsNames[1].text = GameManager.Instance.Competitors[1].PullerName;

        CompetitorsNames[0].color = GameManager.Instance.Competitors[0].PullerColor;
        CompetitorsNames[1].color = GameManager.Instance.Competitors[1].PullerColor;

        CompetitorsNamesBack[0].text = GameManager.Instance.Competitors[0].PullerName;
        CompetitorsNamesBack[1].text = GameManager.Instance.Competitors[1].PullerName;

    }

    private void SetTime(int index)
    {
        float time = GameManager.Instance.CompetitorsTime[index];
        Timers[index].SetTime((int)(GameManager.Instance.CompetitorsTime[index]) / 60, (int)(GameManager.Instance.CompetitorsTime[index] % 60));
    }

    private void AddPoint(int index)
    {
        if (Points.Count < 2) return;
        Points[index].text = $"{StartPointText}{GameManager.Instance.CompetitorsPoints[index]}{EndPointText}";
    }

    protected override void Update()
    {
        if (!CancelReturn) return;
        if (!_viewMenuController.IsViewOnTop(_viewBase)) return;

        if (Input.GetButtonDown("Cancel") && _timeStamp < Time.time)
        {
            if (PauseManager.Instance.CanPause && !PauseManager.Instance.IsPaused)
            {
                _viewMenuController.PushView(PauseView);
                _timeStamp = Time.time + _timeStamp;
            }
        }
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnAddPoint -= AddPoint;
        GameManager.Instance.OnSetTime -= SetTime;
    }
}
