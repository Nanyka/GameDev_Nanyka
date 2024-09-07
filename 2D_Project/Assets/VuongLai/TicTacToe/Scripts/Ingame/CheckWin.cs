using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using V_TicTacToe;

namespace V_TicTacToe
{
    public class CheckWin : MonoBehaviour
    {
        [SerializeField] private V_IntegerStorage currentPlayerId;
        [SerializeField] private int totalCell;

        [Header("Channel")]
        [SerializeField] private V_Vector2Channel checkWinChannel;
        [SerializeField] private V_VoidChannel endTurnChannel;
        [SerializeField] private V_VoidChannel showWinChannel;
        [SerializeField] private V_VoidChannel showDrawChannel;
        [SerializeField] private V_VoidChannel resetLevelChannel;

        private Dictionary<int, List<Vector2>> matrixPositions = new Dictionary<int, List<Vector2>>();

        private void OnEnable()
        {
            checkWinChannel.AddListener(CheckPlayerWin);
            resetLevelChannel.AddListener(OnResetLevel);
        }

        private void OnDisable()
        {
            checkWinChannel.RemoveListener(CheckPlayerWin);
            resetLevelChannel.RemoveListener(OnResetLevel);
        }

        public void CheckPlayerWin(Vector2 matrixPosition)
        {
            bool isWin = false;
            if (!matrixPositions.ContainsKey(currentPlayerId.Value))
            {
                matrixPositions.Add(currentPlayerId.Value, new List<Vector2>() { matrixPosition });
            }
            else
            {
                if (!matrixPositions[currentPlayerId.Value].Contains(matrixPosition))
                {
                    matrixPositions[currentPlayerId.Value].Add(matrixPosition);
                }
            }

            List<Vector2> currentMatrixPositions = matrixPositions[currentPlayerId.Value];

            List<int> horizontalIndexs = new List<int>();
            List<int> verticalIndexs = new List<int>();
            {
                for (int i = 0, max = currentMatrixPositions.Count; i < max; i++)
                {
                    if (currentMatrixPositions[i].y.Equals(matrixPosition.y))
                    {
                        horizontalIndexs.Add((int)currentMatrixPositions[i].x);
                    }

                    if (currentMatrixPositions[i].x.Equals(matrixPosition.x))
                    {
                        verticalIndexs.Add((int)currentMatrixPositions[i].y);
                    }
                }

                if (IsContinuosList(horizontalIndexs) && horizontalIndexs.Count >= 3)
                {
                    isWin = true;
                }
                else
                {
                    if (IsContinuosList(verticalIndexs) && verticalIndexs.Count >= 3)
                    {
                        isWin = true;
                    }
                }
            }

            if (!isWin)
            {
                horizontalIndexs.Clear();

                float matrixPositionTotal = matrixPosition.x + matrixPosition.y;

                float currentMatrixTotal = 0;
                Vector2 currentMatrixPosition = Vector2.zero;
                for (int i = 0, max = currentMatrixPositions.Count; i < max; i++)
                {
                    currentMatrixPosition = currentMatrixPositions[i];
                    currentMatrixTotal = currentMatrixPosition.x + currentMatrixPosition.y;

                    if (currentMatrixTotal.Equals(matrixPositionTotal))
                    {
                        horizontalIndexs.Add((int)currentMatrixPositions[i].x);
                    }
                }

                if (IsContinuosList(horizontalIndexs) && horizontalIndexs.Count >= 3)
                {
                    isWin = true;
                }
            }

            if (!isWin)
            {
                horizontalIndexs.Clear();

                float matrixPositionDifference = matrixPosition.x - matrixPosition.y;

                float currentMatrixDifference = 0;
                Vector2 currentMatrixPosition = Vector2.zero;
                for (int i = 0, max = currentMatrixPositions.Count; i < max; i++)
                {
                    currentMatrixPosition = currentMatrixPositions[i];
                    currentMatrixDifference = currentMatrixPosition.x - currentMatrixPosition.y;

                    if (currentMatrixDifference.Equals(matrixPositionDifference))
                    {
                        horizontalIndexs.Add((int)currentMatrixPositions[i].x);
                    }
                }

                if (IsContinuosList(horizontalIndexs) && horizontalIndexs.Count >= 3)
                {
                    isWin = true;
                }
            }

            if (isWin)
            {
                showWinChannel.RunVoidChannel();
            }
            else
            {
                int totalCurrentCell = 0;
                foreach (var item in matrixPositions)
                {
                    totalCurrentCell += item.Value.Count;
                }

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

        private bool IsContinuosList(List<int> inputList)
        {
            if (inputList == null || inputList.Count <= 1)
            {
                return false;
            }

            bool isContinuosList = false;
            int minValue = -1;
            int maxValue = -1;

            for (int i = 0, max = inputList.Count; i < max; i++)
            {
                if (i == 0)
                {
                    minValue = inputList[i];
                    maxValue = inputList[i];
                }
                else
                {
                    if (inputList[i] < minValue)
                    {
                        minValue = inputList[i];
                    }

                    if (inputList[i] > maxValue)
                    {
                        maxValue = inputList[i];
                    }
                }
            }

            int distanceMinMax = (maxValue - minValue) + 1;

            if (distanceMinMax == inputList.Count)
            {
                isContinuosList = true;
            }

            return isContinuosList;
        }

        private void OnResetLevel()
        {
            matrixPositions.Clear();
        }
    }
}