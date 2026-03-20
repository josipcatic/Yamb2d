using UnityEngine;

public static class GameData
{
    public static int[] diceValues = new int[5];
    public static int[,] scores = new int[14, 4];
    public static bool[,] written = new bool[14, 4];
    public static bool call = false;
    public static int callRow = -1;
    public static int callColumn = 3;
}