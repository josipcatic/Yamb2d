using UnityEngine;

public static class GameData
{
    public static int[] diceValues = new int[5] { 1,1,1,1,1 };
    public static int[,] scores = new int[14, 4];
    public static bool[,] written = new bool[14, 4];
    public static bool canWrite = false;
    public static bool call = false;
    public static int callRow = -1;
    public static int callColumn = 3;
    public static int currentThrow = 0;
}