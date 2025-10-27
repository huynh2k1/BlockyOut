using DG.Tweening;
using GameConfig;
using H_Utils;
using UnityEngine;

public class GameCtrl : BaseGameCtrl
{
    public static GameCtrl I;
    public LevelCtrl levelCtrl;

    public GameState CurState;
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

    public void ChangeState(GameState newState)
    {
        CurState = newState;
    }

    public override void GameHome()
    {
        ChangeState(GameState.None);
        UICtrl.I.Hide(UIType.Game);
        UICtrl.I.Show(UIType.Home);
    }

    public override void GameStart()
    {
        UICtrl.I.ReloadUI();
        UICtrl.I.Hide(UIType.Home);
        UICtrl.I.Show(UIType.Game);
        levelCtrl.OnLevelStart();
        ChangeState(GameState.Playing);
    }

    public override void GameReplay()
    {
        ChangeState(GameState.Playing);
        levelCtrl.OnLevelStart();
    }

    public override void GameNext()
    {
        levelCtrl.OnNextLevel();
        ChangeState(GameState.Playing);
    }

    public override void GameLose()
    {
        SoundManager.I.PlaySoundByType(TypeSound.LOSE);

        UICtrl.I.Show(UIType.Lose);
        ChangeState(GameState.None);
    }

    public override void GameWin()
    {
        PrefData.Coin += 200;
        SoundManager.I.PlaySoundByType(TypeSound.WIN);
        TimeCtrl.I.ResetTimer();
        ChangeState(GameState.None);

        DOVirtual.DelayedCall(1f, () =>
        {
            CoinCtrl.I.UpdateText();
            UICtrl.I.Show(UIType.Win);
        });
    }
}
