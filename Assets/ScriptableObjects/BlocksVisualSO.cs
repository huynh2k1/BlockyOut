using System;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/BlocksVisual", fileName = "BlocksVisual")]
public class BlocksVisualSO : ScriptableObject
{
    public ShapeData[] shapes;
}