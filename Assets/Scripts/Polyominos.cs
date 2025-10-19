using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using H_Utils;
public static class Polyominos
{
    private static readonly int[][,] polyominos = new int[][,]
    {
        new int[,]
        {
            { 0, 0, 1},
            { 0, 0, 1},
            { 1, 1, 1}
        }
    };

    static Polyominos()
    {
        foreach(var polyomino in polyominos)
        {
            ArrUtils.ReverseRows(polyomino);
        }
    }

    public static int[,] Get(int index) => polyominos[index];

    public static int Length => polyominos.Length;

}
