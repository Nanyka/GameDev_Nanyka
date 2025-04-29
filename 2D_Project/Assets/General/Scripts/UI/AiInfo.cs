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
        [SerializeField] private BoolChannel botThinkingChannel;
        
        [SerializeField] private Slider progressSlider;
        [SerializeField] private TextMeshProUGUI[] aiLevelTexts;
        [SerializeField] private GameObject sandClock;

        private void OnEnable()
        {
            resetChannel.AddListener(UpdateAiInfo);
            botThinkingChannel.AddListener(ShowSandClock);
        }

        private void OnDisable()
        {
            resetChannel.RemoveListener(UpdateAiInfo);
            botThinkingChannel.RemoveListener(ShowSandClock);
        }

        private void UpdateAiInfo()
        {
            var level = saveSystemManager.saveData.level;
            progressSlider.value = level + 1;
            for (var i = 0; i < aiLevelTexts.Length; i++)
                aiLevelTexts[i].color = i <= level ? GameConstants.ColorTank[ColorPalate.LightYellow] : 
                    GameConstants.ColorTank[ColorPalate.DarkGreen];
        }
        
        private void ShowSandClock(bool show)
        {
            // Debug.Log($"Show clock: {show}");
            sandClock.SetActive(show);
        }
    }
}