using System;
using TMPro;
using UnityEngine;



public class Scoring : MonoBehaviour
{
    public enum RowType
    {
        Ones,
        Twos,
        Threes,
        Fours,
        Fives,
        Sixes,
        Minimum,
        Maximum,
        TwoPair,
        ThreeOfKind,
        Straight,
        FullHouse,
        Poker,
        Yamb
    }

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI[] cells;

    int[] diceScores;
    bool[,] written = new bool[14, 4];
    int[,] scores = new int[14, 4];

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

    public void SelectCell(int row, int col)
    {
        if (written[row, col])
        {
            Debug.Log("Already written");
            return;
        }

        RowType type = (RowType)row;

        int score = CalculateScore(type);

        scores[row, col] = score;
        written[row, col] = true;

        UpdateText(row, col, score);
    }

    int[] GetCounts()
    {
        int[] counts = new int[7];

        foreach (int d in diceScores)
            counts[d]++;

        return counts;
    }

    int CalculateScore(RowType type)
    {
        int[] counts = GetCounts();

        switch (type)
        {
            case RowType.Ones: return counts[1] * 1;
            case RowType.Twos: return counts[2] * 2;
            case RowType.Threes: return counts[3] * 3;
            case RowType.Fours: return counts[4] * 4;
            case RowType.Fives: return counts[5] * 5;
            case RowType.Sixes: return counts[6] * 6;

            case RowType.Minimum:
                return SumAll();

            case RowType.Maximum:
                return SumAll();

            case RowType.TwoPair:
                return ScoreTwoPair(counts);

            case RowType.ThreeOfKind:
                return ScoreThree(counts);

            case RowType.Straight:
                return ScoreStraight(counts);

            case RowType.FullHouse:
                return ScoreFullHouse(counts);

            case RowType.Poker:
                return ScorePoker(counts);

            case RowType.Yamb:
                return ScoreYamb(counts);
        }

        return 0;
    }

    int SumAll()
    {
        int sum = 0;

        foreach (int d in diceScores)
            sum += d;

        return sum;
    }

    int ScoreTwoPair(int[] counts)
    {
        int pairs = 0;
        int sum = 0;

        for (int i = 6; i >= 1; i--)
        {
            if (counts[i] >= 2)
            {
                pairs++;
                sum += i * 2;
            }
        }

        if (pairs >= 2)
            return sum;

        return 0;
    }

    int ScoreThree(int[] counts)
    {
        for (int i = 1; i <= 6; i++)
        {
            if (counts[i] >= 3)
                return i * 3;
        }

        return 0;
    }

    int ScoreStraight(int[] counts)
    {
        bool small =
            counts[1] == 1 &&
            counts[2] == 1 &&
            counts[3] == 1 &&
            counts[4] == 1 &&
            counts[5] == 1;

        bool big =
            counts[2] == 1 &&
            counts[3] == 1 &&
            counts[4] == 1 &&
            counts[5] == 1 &&
            counts[6] == 1;

        if (small) return 15;
        if (big) return 20;

        return 0;
    }

    int ScoreFullHouse(int[] counts)
    {
        bool three = false;
        bool two = false;
        int sum = 0;

        for (int i = 1; i <= 6; i++)
        {
            if (counts[i] == 3) three = true;
            if (counts[i] == 2) two = true;

            sum += counts[i] * i;
        }

        if (three && two)
            return sum + 25;

        return 0;
    }

    int ScorePoker(int[] counts)
    {
        for (int i = 1; i <= 6; i++)
        {
            if (counts[i] >= 4)
                return i * 4 + 40;
        }

        return 0;
    }

    int ScoreYamb(int[] counts)
    {
        for (int i = 1; i <= 6; i++)
        {
            if (counts[i] == 5)
                return i * 5 + 50;
        }

        return 0;
    }

    void UpdateText(int row, int col, int score)
    {
        int index = row * 4 + col;
        cells[index].text = score.ToString();
    }   
}
