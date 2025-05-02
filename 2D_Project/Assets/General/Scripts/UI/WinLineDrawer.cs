using System;
using System.Collections.Generic;
using AlphaZeroAlgorithm;
using PrimeTween;
using UnityEngine;

namespace TheAiAlchemist
{
    public class WinLineDrawer : MonoBehaviour
    {
        [SerializeField] private BoolChannel endGameChannel;
        [SerializeField] private GameStateStorage gameStateStorage;
        [SerializeField] private VoidChannel resetGameChannel;

        private LineRenderer _lineRenderer;

        private void OnEnable()
        {
            endGameChannel.AddListener(ShowWinningLine);
            resetGameChannel.AddListener(ResetWinningLine);
        }

        private void OnDisable()
        {
            endGameChannel.RemoveListener(ShowWinningLine);
            resetGameChannel.RemoveListener(ResetWinningLine);
        }

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
        }

        private void ShowWinningLine(bool hasWinner)
        {
            if (hasWinner)
            {
                DrawWinningLine(gameStateStorage.GetValue().GetWinningLine());
            }

        }

        private void DrawWinningLine(List<Point> winningPoints)
        {
            if (winningPoints == null || winningPoints.Count < 2) return;

            Point start = winningPoints[0];
            Point end = winningPoints[winningPoints.Count - 1];

            Vector3 startWorld = GetWorldPosition(start);
            Vector3 endWorld = GetWorldPosition(end);

            _lineRenderer.positionCount = 2;
            _lineRenderer.SetPosition(0, startWorld);
            _lineRenderer.SetPosition(1, endWorld);
        }

        // Translate board point (1-based row/col) to world position
        private Vector3 GetWorldPosition(Point point)
        {
            float x = point.Col - 2;
            float y = 2 - point.Row;
            return new Vector3(x, y, 0f);
        }
        
        private void ResetWinningLine()
        {
            _lineRenderer.positionCount = 0;
        }
    }
}