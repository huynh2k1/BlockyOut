using H_Utils;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UIWin : BasePopup
{
    public override UIType Type => UIType.Win;

    [SerializeField] Button _btnHome;
    [SerializeField] Button _btnReplay;

    public static Action OnClickHomeButton;
    public static Action OnClickReplayButton;

    public override void Awake()
    {
        base.Awake();
        _btnHome?.onClick.AddListener(HandleHomeBtnClicked);
        _btnReplay?.onClick.AddListener(HandleReplayClicked);
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
}
