using UnityEngine;

public class PrefData
{
    //public static bool Flag
    //{
    //    get => PlayerPrefs.GetInt(ConstUtils.TESTFLAG, 0) == 0 ? true : false;
    //    set => PlayerPrefs.SetInt(ConstUtils.TESTFLAG, value ? 0 : 1);
    //}
    public static int CurLevel
    {
        get => PlayerPrefs.GetInt(ConstUtils.CURLEVEL, 0);
        set => PlayerPrefs.SetInt(ConstUtils.CURLEVEL, value);
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
    public static string TESTFLAG = "TESTFLAG";
    public static string CURLEVEL = "CURLEVEL";
    public static string TESTSTRING = "TESTSTRING";
}