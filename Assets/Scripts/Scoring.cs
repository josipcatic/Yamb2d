using System;
using TMPro;
using UnityEngine;

public class Scoring : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;

    int[] diceScores;
    void Start()
    {
        scoreText.text = "Your rolled dice: ";
        diceScores = new int[GameData.diceValues.Length];

        for (int i = 0; i < GameData.diceValues.Length; i++)
        {
            diceScores[i] = GameData.diceValues[i];
        }

        Array.Sort(diceScores);

        for (int i = 0; i < GameData.diceValues.Length; i++)
        {
            scoreText.text += diceScores[i].ToString() + " ";
        }
    }

}
