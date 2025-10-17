using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace H_Utils
{
    public abstract class BaseLevel : MonoBehaviour
    {
        public abstract void OnLevelInit();
        public abstract void OnLevelStart();
        public abstract void OnLevelPause();
        public abstract void OnLevelResume();
        public abstract void OnLevelWin();
        public abstract void OnLevelLose();

    }
}
