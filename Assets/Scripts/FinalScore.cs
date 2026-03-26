using TMPro;
using UnityEngine;

public class FinalScore : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI finalScore;

    void Start()
    {
        finalScore.text = $"Your final score is: {CalculateFinalScore()}";
    }

    int CalculateFinalScore()
    {
        int total = 0;

        for (int col = 0; col < GameData.scores.GetLength(1); col++)
        {
            total += CalculateColumn(col);
        }

        return total;
    }
    int CalculateColumn(int col)
    {
        int sum = 0;

        int upperSum = 0;

        // rows 0-5 (ones-sixes)
        for (int row = 0; row <= 5; row++)
        {
            upperSum += GameData.scores[row, col];
        }

        sum += upperSum;

        // bonus +30 if > 60
        if (upperSum > 60)
            sum += 30;


        // min / max rule
        int ones = GameData.scores[0, col];
        int max = GameData.scores[6, col];
        int min = GameData.scores[7, col];

        sum += (max - min) * ones;


        // special rows 8-13
        for (int row = 8; row <= 13; row++)
        {
            sum += GameData.scores[row, col];
        }

        return sum;
    }
}
