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
        GameData.call = false;
        callPanel.SetActive(false);
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

    public void LoadScoreScene()
    {
        SaveDice();
        SceneManager.LoadScene("Write Scene");

    }
}
