using GameConfig;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class DataAllLevel
{
    public List<LevelData> Levels;
}

[Serializable]
public class LevelData 
{
    public int ID;
    public int Rows;
    public int Cols;
    public float Time;
    public CellData[] Cells;
    public BlockData[] Blocks;
}

[Serializable]
public class BlockData
{
    public int id;
    public TransformData position;
    public BlockDir blockDir;
}

[Serializable]
public class CellData
{
    public CellBoardType cellType;
    public ColorType colorType;
}


[Serializable]
public class TransformData
{
    public float x, y, z;

    public TransformData(Vector3 v)
    {
        x = v.x;
        y = v.y;
        z = v.z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }
}

