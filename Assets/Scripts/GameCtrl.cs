using H_Utils;
using UnityEngine;

public class GameCtrl : BaseGameCtrl
{
    public static GameCtrl I;
    public LevelCtrl levelCtrl;

    private void Awake()
    {
        Application.targetFrameRate = 120;
        I = this;
    }

    private void Start()
    {
        GameHome();
    }

    private void OnEnable()
    {
        //UIHome
        UIHome.OnClickPlayButton += GameStart;

        //UIGame
        UIGame.OnClickPauseButton += GameHome;
        UIGame.OnClickReplayButton += GameReplay;

        //UIWin
        UIWin.OnClickHomeButton += GameHome;
        UIWin.OnClickReplayButton += GameReplay;
        UIWin.OnClickNextButton += GameNext;

        //UILose
        UILose.OnClickHomeButton += GameHome;
        UILose.OnClickReplayButton += GameReplay;
    }

    private void OnDisable()
    {
        //UIHome
        UIHome.OnClickPlayButton -= GameStart;

        //UIGame
        UIGame.OnClickPauseButton -= GameHome;
        UIGame.OnClickReplayButton -= GameReplay;

        //UIWin
        UIWin.OnClickHomeButton -= GameHome;
        UIWin.OnClickReplayButton -= GameReplay;
        UIWin.OnClickNextButton -= GameNext;
        
        //UILose    
        UILose.OnClickHomeButton -= GameHome;
        UILose.OnClickReplayButton -= GameReplay;
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
        levelCtrl.OnLevelStart();
    }

    public override void GameNext()
    {
        levelCtrl.OnNextLevel();
    }

    public override void GameLose()
    {
        UICtrl.I.Show(UIType.Lose);
    }

    public override void GameWin()
    {
        UICtrl.I.Show(UIType.Win);
    }
}
