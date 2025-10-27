using System;

namespace GameConfig
{
    public enum GameState
    {
        None,
        Playing,
    }

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
        None = 0,
        Red = 1,
        Green = 2,
        Purple = 3,
        Blue1 = 4,
        Blue2 = 5,
        Yellow = 6,
        Orange = 7
    }
    [Serializable]
    public enum BlockDir
    {
        None = 0,
        Horizontal = 1,
        Vertical = 2
    }
}
