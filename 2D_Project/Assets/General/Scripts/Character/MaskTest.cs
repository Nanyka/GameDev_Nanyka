using System;
using UnityEngine;

namespace TheAiAlchemist
{
    public class MaskTest : MonoBehaviour
    {
        [SerializeField] private IntChannel visualizeMask;
        [SerializeField] private VoidChannel resetMask;
        [SerializeField] protected IndexAndPlotTranslator indexTranslator;

        private IObjectPool _objectPool;

        private void Awake()
        {
            _objectPool = GetComponent<IObjectPool>();
        }

        private void OnEnable()
        {
            visualizeMask.AddListener(VisualEnableState);
            resetMask.AddListener(SetDefaultState);
        }

        private void OnDisable()
        {
            visualizeMask.RemoveListener(VisualEnableState);
            resetMask.RemoveListener(SetDefaultState);
        }

        private void VisualEnableState(int index)
        {
            var plotSelecting = index / 3;
            var prioritySelecting = index % 3;
            var maskObj = _objectPool.GetObject();
            maskObj.transform.position = indexTranslator.IndexToPlot(plotSelecting);
            maskObj.SetActive(true);
        }
        
        private void SetDefaultState()
        {
            _objectPool.ResetPool();
        }
    }
}