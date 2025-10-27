using System;
using UnityEngine;
using UnityEngine.UI;

public class CoinCtrl : MonoBehaviour
{
    public static CoinCtrl I;
    [SerializeField] Text coinText;


    void Start()
    {
        I = this;
    }

    public void UpdateText()
    {
        coinText.text = PrefData.Coin.ToString("N0");
        // "N0" => định dạng số có dấu phẩy ngăn cách hàng nghìn, không có số lẻ
    }
}
