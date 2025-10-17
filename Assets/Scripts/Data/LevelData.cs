using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelData 
{
    public int rows;
    public int cols;
    public List<BlockData> blocks = new List<BlockData>();
}

[Serializable]
public class BlockData
{
    public int id;
    public int row;
    public int col;
    public int width;
    public int height;
    public int rotation;
}
