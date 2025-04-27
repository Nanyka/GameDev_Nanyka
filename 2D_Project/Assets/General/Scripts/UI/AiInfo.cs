using System;
using TMPro;
using UnityEngine;

namespace TheAiAlchemist
{
    public class AiInfo : MonoBehaviour
    {
        [SerializeField] private VoidChannel resetChannel;
        [SerializeField] private SaveSystemManager saveSystemManager;

        [SerializeField] private TextMeshProUGUI aiName;

        private void OnEnable()
        {
            resetChannel.AddListener(UpdateAiInfo);
        }

        private void OnDisable()
        {
            resetChannel.AddListener(UpdateAiInfo);
        }

        private void UpdateAiInfo()
        {
            aiName.text = $"I am bot level {saveSystemManager.saveData.level}";
        }
    }
}