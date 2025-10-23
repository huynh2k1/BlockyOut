using System;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/BlocksVisual", fileName = "BlocksVisual")]
public class BlocksVisualSO : ScriptableObject
{
    public ShapeData[] shapes;

    public ShapeData GetShapeDataByID(int id) => shapes[id];
}