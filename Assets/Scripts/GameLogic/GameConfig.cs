using System;

namespace GameConfig
{
    [Serializable]
    public enum CellBoardType
    {
        Field = 0, //Phần bảng chơi
        Border = 1, //Phần viền
        Empty = 2, //None
    }

    [Serializable]
    public enum ColorType
    {
        None,
        Red,
        Green,
        Purple,
        Blue1,
        Blue2,
        Yellow,
        Orange
    }
    [Serializable]
    public enum BlockDir
    {
        None,
        Horizontal,
        Vertical
    }
}
