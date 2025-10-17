using UnityEngine;
namespace H_Utils
{
    public abstract class BaseUI : MonoBehaviour
    {
        //UI kế thừa phải override lại UIType VD: public override UIType Type => UIType.Home;
        public abstract UIType Type { get; }    
        public virtual void Show() => gameObject.SetActive(true);
        public virtual void Hide() => gameObject.SetActive(false);
    }
    public enum UIType
    {
        Home,
        Game,
        Win,
        Lose,
        Pause,
        Setting,
        HowToPlay,
        Loading
    }

}
