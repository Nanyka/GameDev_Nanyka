using System;
using AlphaZeroAlgorithm;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TheAiAlchemist
{
    public class AiInfo : MonoBehaviour
    {
        [SerializeField] private VoidChannel resetChannel;
        [SerializeField] private SaveSystemManager saveSystemManager;
        [SerializeField] private Slider progressSlider;
        [SerializeField] private TextMeshProUGUI[] aiLevelTexts;

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
            var level = saveSystemManager.saveData.level;
            progressSlider.value = level + 1;
            for (var i = 0; i < aiLevelTexts.Length; i++)
                aiLevelTexts[i].color = i <= level ? GameConstants.ColorTank[ColorPalate.LightYellow] : 
                    GameConstants.ColorTank[ColorPalate.DarkGreen];
        }
    }
}