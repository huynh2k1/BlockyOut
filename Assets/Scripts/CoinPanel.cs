using UnityEngine;
using UnityEngine.UI;

public class CoinPanel : MonoBehaviour
{
    [SerializeField] Text coinText;
    private int coin = 15723;

    void Start()
    {
        UpdateText(coin);
    }

    void UpdateText(int value)
    {
        coinText.text = value.ToString("N0");
        // "N0" => định dạng số có dấu phẩy ngăn cách hàng nghìn, không có số lẻ
    }
}
