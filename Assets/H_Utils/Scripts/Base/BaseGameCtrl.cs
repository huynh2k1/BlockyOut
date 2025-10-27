using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseGameCtrl : MonoBehaviour
{
    public virtual void GameHome() { }
    public virtual void GameStart() { }
    public virtual void GamePause() { }
    public virtual void GameResume() { }
    public virtual void GameReplay() { }
    public virtual void GameWin() { }
    public virtual void GameLose() { }
    public virtual void GameNext() { }
}
