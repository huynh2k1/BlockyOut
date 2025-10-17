using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using H_Utils;
public class UICtrl : BaseUICtrl
{
    public static UICtrl I;
    protected override void Awake()
    {
        base.Awake();
        I = this;
    }

    private void OnEnable()
    {
        UIHome.OnClickHowToPlayButton += ShowHowToPlay;
    }

    private void OnDisable()
    {
        UIHome.OnClickHowToPlayButton -= ShowHowToPlay;
    }

    public void ShowHowToPlay()
    {
        Show(UIType.HowToPlay);
    }
}
