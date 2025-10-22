using H_Utils;
using UnityEngine;

public class GameCtrl : BaseGameCtrl
{
    public static GameCtrl I;
    public LevelCtrl levelCtrl;

    private void Awake()
    {
        I = this;
    }

    private void Start()
    {
        GameHome();
    }

    private void OnEnable()
    {
        UIHome.OnClickPlayButton += GameStart;
        UIGame.OnClickPauseButton += GameHome;

        UIWin.OnClickHomeButton += GameHome;
        UIWin.OnClickReplayButton += GameReplay;
    }

    private void OnDisable()
    {
        UIHome.OnClickPlayButton -= GameStart;
        UIGame.OnClickPauseButton -= GameHome;

        UIWin.OnClickHomeButton -= GameHome;
        UIWin.OnClickReplayButton -= GameReplay;    
    }

    public override void GameHome()
    {
        UICtrl.I.Hide(UIType.Game);
        UICtrl.I.Show(UIType.Home);
    }

    public override void GameStart()
    {
        UICtrl.I.Hide(UIType.Home);
        UICtrl.I.Show(UIType.Game);
        levelCtrl.OnLevelStart();
    }

    public override void GameReplay()
    {
        
    }

    public override void GamePause()
    {
    }

    public override void GameResume()
    {
    }

    public override void GameLose()
    {
    }

    public override void GameWin()
    {
    }
}
