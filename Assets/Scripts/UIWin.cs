using H_Utils;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UIWin : BasePopup
{
    public override UIType Type => UIType.Win;

    [SerializeField] Button _btnHome;
    [SerializeField] Button _btnReplay;
    [SerializeField] Button _btnNext;

    public static Action OnClickHomeButton;
    public static Action OnClickReplayButton;
    public static Action OnClickNextButton;

    public override void Awake()
    {
        base.Awake();
        _btnHome?.onClick.AddListener(HandleHomeBtnClicked);
        _btnReplay?.onClick.AddListener(HandleReplayClicked);
        _btnNext?.onClick.AddListener(HandleNextClicked);
    }

    void HandleHomeBtnClicked()
    {
        Hide();
        OnClickHomeButton?.Invoke();
    }

    void HandleReplayClicked()
    {
        Hide();
        OnClickReplayButton?.Invoke();  
    }

    void HandleNextClicked()
    {
        Hide();
        OnClickNextButton?.Invoke();
    }
}
