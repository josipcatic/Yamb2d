using System;
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
    [SerializeField] Image callImage;
    [SerializeField] GameObject callPanel;
    [SerializeField] Sprite[] diceSprites;

    int min, max, currentNumber, maximumThrows, currentTrow;
    bool[] isLocked;

    Color lockedColor = Color.gray;
    Color unlockedColor = Color.white;
    void Start()
    {
        min = 1;
        max = 7;
        maximumThrows = 3;
        currentTrow = GameData.currentThrow;
        isLocked = new bool[dice.Length];
        DisplayDice();
        UpdateThrowText();
        GameData.call = false;
        callPanel.SetActive(false);
    }

    public void Roll()
    {
        for (int i = 0; i < dice.Length; i++)
        {
            if (isLocked[i] == false && currentTrow < maximumThrows)
            {
                currentNumber = UnityEngine.Random.Range(min, max);
                GameData.diceValues[i] = currentNumber;
                diceImages[i].sprite = diceSprites[currentNumber - 1 ];
                
            }
        }
        currentTrow++;
        GameData.currentThrow = currentTrow;
        UpdateThrowText();

    }

    void UpdateThrowText()
    {
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
        if (currentTrow == 0)
            return;
        isLocked[i] = !isLocked[i];

        if (isLocked[i])
            diceImages[i].color = lockedColor;
        else
            diceImages[i].color = unlockedColor;
    }

    public void Call()
    {
        if (currentTrow <= 1)
        {
            ShowCallButtons();
        }
    }

    void ShowCallButtons()
    {
        callPanel.SetActive(true);
    }

    public void SelectCallRow(int row)
    {
        GameData.call = true;
        GameData.callRow = row;

        callPanel.SetActive(false);

        callImage.color = Color.green;
    }


    public void SaveDice()
    {
        for (int i = 0; i < dice.Length; i++)
        {
            GameData.diceValues[i] = int.Parse(dice[i].text);
        }
    }

    public void DisplayDice()
    {
        for (int i = 0; i < dice.Length;i++)
        {
            diceImages[i].sprite = diceSprites[GameData.diceValues[i] - 1];
        }
    }

    public void LoadScoreScene()
    {
        Scoring.isTableView = false;
        if (currentTrow > 0)
        {
            GameData.canWrite = true;            
        }
        SceneManager.LoadScene("Write Scene");

    }

    public void LoadTableScene()
    {
        Scoring.isTableView = true;
        SceneManager.LoadScene("Write Scene");
    }
}
