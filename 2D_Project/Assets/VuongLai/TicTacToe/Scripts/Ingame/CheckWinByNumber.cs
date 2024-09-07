using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using V_TicTacToe;

public class CheckWinByNumber : MonoBehaviour
{
    [SerializeField] private Vector2Int matrixValue = new Vector2Int(3, 3);

    [Header("Storage")]
    [SerializeField] private V_IntegerStorage currentPlayerId;
    [SerializeField] private V_IntegerStorage ticTacToeScore;

    [Header("Channel")]
    [SerializeField] private V_IntegerChannel checkWinByNumberChannel;
    [SerializeField] private V_VoidChannel endTurnChannel;
    [SerializeField] private V_VoidChannel showWinChannel;
    [SerializeField] private V_VoidChannel showDrawChannel;
    [SerializeField] private V_VoidChannel resetLevelChannel;
    [SerializeField] private V_IntegerChannel updatePlayer1ScoreChannel;

    private List<int> player1Numbers = new List<int>();
    private List<int> player2Numbers = new List<int>();

    private int totalCell;

    private void Awake()
    {
        totalCell = matrixValue.x * matrixValue.y;
    }

    private void OnEnable()
    {
        checkWinByNumberChannel.AddListener(CheckWin);
        resetLevelChannel.AddListener(OnResetLevel);
    }

    private void OnDisable()
    {
        checkWinByNumberChannel.RemoveListener(CheckWin);
        resetLevelChannel.RemoveListener(OnResetLevel);
    }

    private void OnResetLevel()
    {
        player1Numbers.Clear();
        player2Numbers.Clear();
    }

    public void CheckWin(int currentNumber)
    {
        List<int> currentNumbers = new List<int>();

        if (currentPlayerId.Value.Equals(0))
        {
            currentNumbers = player1Numbers;
        }
        else if (currentPlayerId.Value.Equals(1))
        {
            currentNumbers = player2Numbers;
        }

        bool isWin = false;

        if (!currentNumbers.Contains(currentNumber))
        {
            currentNumbers.Add(currentNumber);
        }

        List<int> horizontalNumbers = GetHorizontalNumbers(currentNumber);
        if (horizontalNumbers.Count >= matrixValue.x)
        {
            isWin = CheckListHasContainList(currentNumbers, horizontalNumbers);
        }

        if (!isWin)
        {
            List<int> verticalNumbers = GetVerticalNumbers(currentNumber);
            if (verticalNumbers.Count >= matrixValue.y)
            {
                isWin = CheckListHasContainList(currentNumbers, verticalNumbers);
            }
        }

        if (!isWin)
        {
            List<int> mainDiagonalNumbers = GetMainDiagonalNumbers(currentNumber);
            if (mainDiagonalNumbers.Count >= matrixValue.y)
            {
                isWin = CheckListHasContainList(currentNumbers, mainDiagonalNumbers);
            }
        }

        if (!isWin)
        {
            List<int> secondDiagonalNumbers = GetSecondDiagonalNumbers(currentNumber);
            if (secondDiagonalNumbers.Count >= matrixValue.y)
            {
                isWin = CheckListHasContainList(currentNumbers, secondDiagonalNumbers);
            }
        }

        //Check Result
        if (isWin)
        {
            showWinChannel.RunVoidChannel();

            if (currentPlayerId.Value.Equals(0))
            {
                ticTacToeScore.Value += 10;
            }
            else if (currentPlayerId.Value.Equals(1))
            {
                ticTacToeScore.Value -= 10;

                if (ticTacToeScore.Value < 0)
                {
                    ticTacToeScore.Value = 0;
                }
            }

            updatePlayer1ScoreChannel.RunIntegerChannel(ticTacToeScore.Value);
        }
        else
        {
            int totalCurrentCell = 0;

            totalCell += player1Numbers.Count;
            totalCell += player2Numbers.Count;

            if (totalCurrentCell >= totalCell)
            {
                showDrawChannel.RunVoidChannel();
            }
            else
            {
                endTurnChannel.RunVoidChannel();
            }
        }
    }

    public bool CheckListHasContainList(List<int> listContain, List<int> listForCheck)
    {
        if (listContain == null || listContain.Count <= 0)
        {
            return false;
        }

        if (listForCheck == null || listForCheck.Count <= 0)
        {
            return false;
        }

        bool hasContain = true;
        for (int i = 0, length = listForCheck.Count; i < length; i++)
        {
            if (!listContain.Contains(listForCheck[i]))
            {
                hasContain = false;
                break;
            }
        }

        return hasContain;
    }

    public List<int> GetHorizontalNumbers(int currentNumber)
    {
        int yValue = (int)(currentNumber / matrixValue.x);
        int xValue = currentNumber % matrixValue.x;

        List<int> horizontalNumbers = new List<int>();
        int currentNumberInList = 0;
        for (int i = 0, length = matrixValue.x; i < length; i++)
        {
            currentNumberInList = (yValue * matrixValue.x) + i;
            horizontalNumbers.Add(currentNumberInList);
        }

        return horizontalNumbers;
    }

    public List<int> GetVerticalNumbers(int currentNumber)
    {
        int yValue = (int)(currentNumber / matrixValue.x);
        int xValue = currentNumber % matrixValue.x;

        List<int> verticalNumbers = new List<int>();
        int currentNumberInList = 0;
        for (int i = 0, length = matrixValue.y; i < length; i++)
        {
            currentNumberInList = xValue + (i * matrixValue.x);
            verticalNumbers.Add(currentNumberInList);
        }

        return verticalNumbers;
    }

    public List<int> GetMainDiagonalNumbers(int currentNumber)
    {
        int yValue = (int)(currentNumber / matrixValue.x);
        int xValue = currentNumber % matrixValue.x;

        List<int> mainDiagonalNumbers = new List<int>();

        int fisrtNumber = currentNumber - ((matrixValue.x + 1) * yValue);
        int currentNumberInList = 0;

        //int lengthList = Mathf.Clamp((matrixValue.x - xValue), 0, matrixValue.y);
        int lengthList = matrixValue.y;
        for (int i = /*xValue*/0; i < lengthList; i++)
        {
            currentNumberInList = fisrtNumber + (i * (matrixValue.x + 1));
            if (currentNumberInList >= (i * matrixValue.x) && currentNumberInList <= ((i * matrixValue.x) + (matrixValue.x - 1)))
                mainDiagonalNumbers.Add(currentNumberInList);
        }

        return mainDiagonalNumbers;
    }

    public List<int> GetSecondDiagonalNumbers(int currentNumber)
    {
        int yValue = (int)(currentNumber / matrixValue.x);
        int xValue = currentNumber % matrixValue.x;

        List<int> secondDiagonalNumbers = new List<int>();
        int firstNumber = currentNumber - ((matrixValue.x - 1) * yValue);
        int currentNumberInList = 0;

        //int listLength = Mathf.Clamp((xValue + 1), 0, matrixValue.y);
        int listLength = matrixValue.y;
        for (int i = 0; i < listLength; i++)
        {
            currentNumberInList = firstNumber + (i * (matrixValue.y - 1));
            if (currentNumberInList >= (i * matrixValue.x) && currentNumberInList <= ((i * matrixValue.x) + (matrixValue.x - 1)))
                secondDiagonalNumbers.Add(currentNumberInList);
        }

        return secondDiagonalNumbers;
    }
}
