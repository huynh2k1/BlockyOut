using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseGameCtrl : MonoBehaviour
{
    public abstract void GameHome();
    public abstract void GameStart();
    public abstract void GamePause();
    public abstract void GameResume();
    public abstract void GameReplay();
    public abstract void GameWin();
    public abstract void GameLose();
}
