using TMPro;
using UnityEngine;

public class FinalScore : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI finalScore;
    void Start()
    {
        finalScore.text = $"Your final score is: {Sum(GameData.scores).ToString()}";
    }

    int Sum(int[,] scores)
    {
        int sum = 0;
        for (int i = 0; i < scores.GetLength(0); i++)
        {
            for(int j = 0; j < scores.GetLength(1); j++)
            {
                sum += scores[i, j];
            }
        }
        return sum;
    }
}
