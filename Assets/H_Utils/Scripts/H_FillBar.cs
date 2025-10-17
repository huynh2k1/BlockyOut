using UnityEngine;
using UnityEngine.UI;
namespace H_Utils
{
    public class H_FillBar : MonoBehaviour
    {
        [SerializeField] Image _fillImage;
        [SerializeField] Text _text;


        public void UpdateFillBar(float value)
        {
            _fillImage.fillAmount = value;  
        }

        public void UpdateText(string str)
        {
            _text.text = str;
        }
    }
}
