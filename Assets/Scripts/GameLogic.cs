using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI[] dice;
    [SerializeField] TextMeshProUGUI throwsLeft;
    [SerializeField] Image[] diceImages;

    int min, max, currentNumber, maximumThrows, currentTrow;
    bool[] isLocked;

    Color lockedColor = Color.gray;
    Color unlockedColor = Color.white;
    void Start()
    {
        min = 1;
        max = 7;
        maximumThrows = 3;
        currentTrow = 0;
        isLocked = new bool[dice.Length];
        Roll();
        currentTrow--;
        throwsLeft.text = "Current throw: " + (currentTrow % 3).ToString();
    }

    public void Roll()
    {
        for (int i = 0; i < dice.Length; i++)
        {
            if (isLocked[i] == false && currentTrow < maximumThrows)
            {
                currentNumber = Random.Range(min, max);
                dice[i].text = currentNumber.ToString();
                
            }
        }
        currentTrow++;
        if (currentTrow < maximumThrows)
        {
            throwsLeft.text = "Current throw: " + currentTrow.ToString();
        }
        else
        {
            throwsLeft.text = "Cannot roll more.";
        }

    }

    public void Lock(int i)
    {
        isLocked[i] = !isLocked[i];

        if (isLocked[i])
            diceImages[i].color = lockedColor;
        else
            diceImages[i].color = unlockedColor;
    }

    public void SaveDice()
    {
        for (int i = 0; i < dice.Length; i++)
        {
            GameData.diceValues[i] = int.Parse(dice[i].text);
        }
    }

    public void LoadScoreScene()
    {
        SaveDice();
        SceneManager.LoadScene("Write Scene");

    }
}
