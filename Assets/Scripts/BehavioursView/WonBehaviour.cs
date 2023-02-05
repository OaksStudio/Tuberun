using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class WonBehaviour : ViewBehaviour
{

    [TitleGroup("General Setups")]
    public TextMeshProUGUI GameModeText;
    public TextMeshProUGUI RetryButton;
    public string PointsText = "points";
    public string TimeText = "s";

    [TitleGroup("First Competitor")]
    public TextMeshProUGUI FirstCompetitor;
    public TextMeshProUGUI FirstPoints;
    public TextMeshProUGUI FirstTime;
    public TextMeshProUGUI FirstResultMessage;

    [Title("Holders")]
    public RectTransform FirstPointsHolder;
    public RectTransform FirstTimeHolder;
    public RectTransform FirstResultMessageHolder;


    [TitleGroup("Second Competitor")]
    public TextMeshProUGUI SecondCompetitor;
    public TextMeshProUGUI SecondPoints;
    public TextMeshProUGUI SecondTime;
    public TextMeshProUGUI SecondResultMessage;

    [Title("Holders")]
    public RectTransform SecondPointsHolder;
    public RectTransform SecondTimeHolder;
    public RectTransform SecondResultMessageHolder;


    private void Start()
    {
        GameManager.Instance.OnWon += Call;
    }

    private void Call(SOPuller winner)
    {
        SOPuller first = GameManager.Instance.Competitors[0];
        SOPuller second = GameManager.Instance.Competitors[1];

        FirstCompetitor.text = first.PullerName;
        SecondCompetitor.text = second.PullerName;
        FirstCompetitor.color = first.PullerColor;
        SecondCompetitor.color = second.PullerColor;

        FirstPoints.text = $"{GameManager.Instance.CompetitorsPoints[0]} {PointsText}";
        SecondPoints.text = $"{GameManager.Instance.CompetitorsPoints[1]} {PointsText}";

        RetryButton.text = $"{GameManager.Instance.GameMode.GameModeInfo.RematchText}";

        if (GameManager.Instance.GameMode.GameModeInfo.Showtime)
        {
            SecondTimeHolder.gameObject.SetActive(true);
            FirstTimeHolder.gameObject.SetActive(true);

            FirstTime.text = $"{GetTime(GameManager.Instance.CompetitorsTime[0])} {TimeText}";
            SecondTime.text = $"{GetTime(GameManager.Instance.CompetitorsTime[1])} {TimeText}";
        }
        else
        {
            SecondTimeHolder.gameObject.SetActive(false);
            FirstTimeHolder.gameObject.SetActive(false);
        }

        if (GameManager.Instance.GameMode.GameModeInfo.ShowResultMessage)
        {
            if (winner.Equals(first))
            {
                FirstResultMessage.text = GameManager.Instance.GameMode.GameModeInfo.ResultMessage;
                FirstResultMessageHolder.gameObject.SetActive(true);
                SecondResultMessageHolder.gameObject.SetActive(false);
            }
            else
            {
                SecondResultMessage.text = GameManager.Instance.GameMode.GameModeInfo.ResultMessage;
                FirstResultMessageHolder.gameObject.SetActive(false);
                SecondResultMessageHolder.gameObject.SetActive(true);
            }
        }
        else
        {
            FirstResultMessageHolder.gameObject.SetActive(false);
            SecondResultMessageHolder.gameObject.SetActive(false);
        }

        _viewMenuController.PushView(_viewBase);
    }

    private string GetTime(float time)
    {
        return $"{(time / 60).ToString("00")}:{(time % 60).ToString("00")}";
    }

    protected override void OnEnter()
    {
        base.OnEnter();
    }
}
