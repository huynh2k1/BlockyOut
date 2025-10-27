using UnityEngine;

public class PrefData
{
    public static bool Music
    {
        get => PlayerPrefs.GetInt(ConstUtils.MUSIC, 0) == 0 ? true : false;
        set => PlayerPrefs.SetInt(ConstUtils.MUSIC, value ? 0 : 1);
    }
    public static bool Sound
    {
        get => PlayerPrefs.GetInt(ConstUtils.SOUND, 0) == 0 ? true : false;
        set => PlayerPrefs.SetInt(ConstUtils.SOUND, value ? 0 : 1);
    }

    public static int CurLevel
    {
        get => PlayerPrefs.GetInt(ConstUtils.CURLEVEL, 0);
        set => PlayerPrefs.SetInt(ConstUtils.CURLEVEL, value);
    } 

    public static int Coin
    {
        get => PlayerPrefs.GetInt(ConstUtils.COIN, 0);
        set => PlayerPrefs.SetInt(ConstUtils.COIN, value);
    }
}

/// <summary>
/// STATIC CLASS 
/// Không thể kế thừa hoặc bị kế thừa
/// Không thế khởi tạo new Class()
/// Chỉ chứa static members
/// </summary>

public class ConstUtils
{
    public static string CURLEVEL = "CURLEVEL";
    public static string COIN = "COIN";
    public static string MUSIC = "MUSIC";
    public static string SOUND = "SOUND";
}