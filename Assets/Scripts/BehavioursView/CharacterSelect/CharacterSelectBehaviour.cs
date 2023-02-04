using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterSelectBehaviour : ViewBehaviour
{
    public CompetitorView Competitor_1;
    public CompetitorView Competitor_2;

    protected override void OnEnter()
    {
        base.OnEnter();
    
        Competitor_1.SetJoinSetup(CharacterSelect.Instance.DefaultPlayers[1]);
        Competitor_2.SetJoinSetup(CharacterSelect.Instance.DefaultPlayers[2]);
    }

    protected override void OnExit()
    {
        base.OnExit();
    }


}
