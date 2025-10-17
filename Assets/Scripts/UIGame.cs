using H_Utils;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UIGame : BaseUI
{
    [SerializeField] Button _btnReplay;
    [SerializeField] Button _btnPause;

    public override UIType Type => UIType.Game;

    public static Action OnClickReplayButton;
    public static Action OnClickPauseButton;

    private void Awake()
    {
        _btnReplay?.onClick.AddListener(HandleReplayBtnClicked);
        _btnPause?.onClick.AddListener(HandlePauseBtnClicked);
    }

    public void HandleReplayBtnClicked()
    {
        OnClickReplayButton?.Invoke();
    }

    public void HandlePauseBtnClicked()
    {
        OnClickPauseButton?.Invoke();
    }
}
