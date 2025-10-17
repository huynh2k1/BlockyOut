using H_Utils;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UIHome : BaseUI
{
    [SerializeField] Button _btnPlay;
    [SerializeField] Button _btnHowToPlay;

    public override UIType Type => UIType.Home;

    public static Action OnClickPlayButton;
    public static Action OnClickHowToPlayButton;

    private void Awake()
    {
        _btnPlay?.onClick.AddListener(HandlePlayBtnClicked);
        _btnHowToPlay?.onClick.AddListener(HandleHowToPlayBtnClicked);   
    }

    public void HandlePlayBtnClicked()
    {
        Hide();
        OnClickPlayButton?.Invoke();
    }

    public void HandleHowToPlayBtnClicked()
    {
        OnClickHowToPlayButton?.Invoke();
    }
}
