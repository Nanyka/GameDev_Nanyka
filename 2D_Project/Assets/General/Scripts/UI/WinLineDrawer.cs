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
        private float _drawDuration = 0.5f;

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
                AnimateWinningLine(gameStateStorage.GetValue().GetWinningLine());
        }

        private void AnimateWinningLine(List<Point> winningPoints)
        {
            if (winningPoints == null || winningPoints.Count < 2) return;

            Point start = winningPoints[0];
            Point end = winningPoints[^1];

            Vector3 startWorld = GetWorldPosition(start);
            Vector3 endWorld = GetWorldPosition(end);
            
            // 1) Build world‐space positions from your Point list
            Vector3[] pts = {startWorld, endWorld};
            int n = pts.Length;

            // 2) Compute each segment's length and the total
            var segLengths = new float[n - 1];
            float totalLength = 0f;
            for (int i = 0; i < n - 1; i++)
            {
                segLengths[i] = Vector3.Distance(pts[i], pts[i + 1]);
                totalLength += segLengths[i];
            }

            // 3) Prepare a fixed‐size draw buffer and initialize the first point
            var drawBuffer = new Vector3[n];
            drawBuffer[0] = pts[0];
            _lineRenderer.positionCount = 1;
            _lineRenderer.SetPosition(0, drawBuffer[0]);

            // 4) Create a PrimeTween sequence (defaults to linear)
            var seq = Sequence.Create();

            for (int i = 0; i < segLengths.Length; i++)
            {
                int idx = i; // capture for the closures

                // a) open the next vertex slot via callback
                seq.ChainCallback(() =>
                {
                    _lineRenderer.positionCount = idx + 2;
                    drawBuffer[idx + 1] = pts[idx];
                    _lineRenderer.SetPosition(idx + 1, drawBuffer[idx + 1]);
                });

                // b) tween that new vertex along its segment, with linear easing
                float segDur = segLengths[idx] / totalLength * _drawDuration;
                seq.Chain(
                    Tween.Custom(
                        0f,
                        1f,
                        duration:      segDur,
                        ease:          Ease.Linear,
                        onValueChange: t =>
                        {
                            drawBuffer[idx + 1] = Vector3.Lerp(pts[idx],pts[idx + 1], t);
                            _lineRenderer.SetPosition(idx + 1, drawBuffer[idx + 1]);
                        }
                    )
                );
            }
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