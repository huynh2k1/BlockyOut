using GameConfig;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/ColorTypeConfig", fileName = "ColorTypeConfig")]
public class ColorTypeSO : ScriptableObject
{
    public Color colorNone;
    public Color colorRed;
    public Color colorGreen;
    public Color colorPurple;
    public Color colorBlue1;
    public Color colorBlue2;
    public Color colorYellow;
    public Color colorOrange;


    public Color ConvertColorTypeToColor(ColorType color)
    {
        switch (color)
        {
            case ColorType.None:
                return Color.black;
            case ColorType.Red:
                return colorRed;
            case ColorType.Green:
                return colorGreen;
            case ColorType.Purple:
                return colorPurple;
            case ColorType.Blue1:
                return colorBlue1;
            case ColorType.Blue2:
                return colorBlue2;
            case ColorType.Yellow:
                return colorYellow;
            case ColorType.Orange:
                return colorOrange;
            default:
                return default;

        }
    }
}
