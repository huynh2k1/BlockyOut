using H_Utils;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UIGame : BaseUI
{
    [SerializeField] Button _btnReplay;
    [SerializeField] Button _btnPause;
    [SerializeField] Button _btnPauseTime;
    [SerializeField] GameObject _pauseClickedUI;
    [SerializeField] Text _textLevel;
    public override UIType Type => UIType.Game;

    public static Action OnClickReplayButton;
    public static Action OnClickPauseButton;

    private void Awake()
    {
        _btnReplay?.onClick.AddListener(HandleReplayBtnClicked);
        _btnPause?.onClick.AddListener(HandlePauseBtnClicked);
        _btnPauseTime?.onClick.AddListener(HandlePauseTimeBtnClicked);
    }

    public void ReloadUI()
    {
        CheckPauseTimeCanClick();
        _textLevel.text = $"Level {PrefData.CurLevel + 1}";
    }

    public void HandleReplayBtnClicked()
    {
        OnClickReplayButton?.Invoke();
    }

    public void HandlePauseBtnClicked()
    {
        OnClickPauseButton?.Invoke();
    }

    public void HandlePauseTimeBtnClicked()
    {
        if (PrefData.Coin < 200)
            return;

        PrefData.Coin -= 200;
        CoinCtrl.I.UpdateText();

        ShowPauseClickedUI(true);
        _btnPauseTime.interactable = false;

        TimeCtrl.I.PauseTimer(10f, () =>
        {
            CheckPauseTimeCanClick();
        });
    }

    public void CheckPauseTimeCanClick()
    {
        bool isShow = (PrefData.Coin >= 200) ? false : true;
        _btnPauseTime.interactable = !isShow;
        ShowPauseClickedUI(isShow);
    }

    public void ShowPauseClickedUI(bool isShow)
    {
        _pauseClickedUI.SetActive(isShow);
    }
}
