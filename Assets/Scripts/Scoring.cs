using System;
using TMPro;
using UnityEngine;

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

public enum ColumnType
{
    Down,
    Up,
    Free,
    Call
}


public class Scoring : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI[] cells;

    int[] diceScores;
    bool canWrite = false;

    void Start()
    {
        canWrite = true;
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

        RestoreTable();
    }

    void RestoreTable()
    {
        for (int r = 0; r < 14; r++)
        {
            for (int c = 0; c < 4; c++)
            {
                if (GameData.written[r, c])
                {
                    UpdateText(r, c, GameData.scores[r, c]);
                }
            }
        }
    }

    public void SelectCell(int index)
    {
        int row = index / 4;
        int col = index % 4;

        ColumnType columnType = (ColumnType)col;

        if (GameData.written[row, col])
        {
            Debug.Log("Already written");
            return;
        }

        if (!IsMoveAllowed(row, col, columnType))
        {
            Debug.Log("Not allowed in this column");
            return;
        }

        RowType type = (RowType)row;

        int score = CalculateScore(type);

        GameData.scores[row, col] = score;
        GameData.written[row, col] = true;

        GameData.call = false;
        GameData.callRow = -1;

        UpdateText(row, col, score);

        canWrite = false;
    }

    bool IsMoveAllowed(int row, int col, ColumnType columnType)
    {
        if (canWrite)
        {
            switch (columnType)
            {
                case ColumnType.Down:
                    return CheckDown(row, col);

                case ColumnType.Up:
                    return CheckUp(row, col);

                case ColumnType.Free:
                    return CheckBoth(row,col);

                case ColumnType.Call:
                    return CheckCall(row, col);
            }
        }
        return false;
    }

    bool CheckDown(int row, int col)
    {
        if (GameData.call)
            return false;
        if (row == 0)
            return true;
   
        return GameData.written[row - 1, col];
    }
    bool CheckBoth(int row, int col)
    {
        if (GameData.call)
            return false;

        return true;
    }

    bool CheckUp(int row, int col)
    {
        if (GameData.call)
            return false;
        if (row == 13)
            return true;

        return GameData.written[row + 1, col];
    }
    bool CheckCall(int row, int col)
    {
        if (!GameData.call)
            return false;

        if (col != GameData.callColumn)
            return false;

        if (row != GameData.callRow)
            return false;

        return true;
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

        if (small) return 35;
        if (big) return 45;

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
