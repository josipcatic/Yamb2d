using UnityEngine;

public class ResetGame : MonoBehaviour
{
    void Start()
    {
        ResetScores(GameData.scores);
        ResetScores(GameData.written);
        GameData.currentThrow = 0;
    }

    void ResetScores(int[,] scores)
    {
        for (int i = 0; i < 14; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                scores[i,j] = 0;
            }
        }
    }

    void ResetScores(bool[,] scores)
    {
        for (int i = 0; i < 14; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                scores[i, j] = false;
            }
        }
    }

}
