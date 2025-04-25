using System;
using AlphaZeroAlgorithm;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace TheAiAlchemist
{
    public class SimpleUnit : MonoBehaviour, IPiece
    {
        [SerializeField] private IntStorage askUnitIndex;
        [SerializeField] private GameStateStorage currentState;
        [SerializeField] private VoidChannel changePlayerChannel;
        [SerializeField] private VoidChannel resetGameChannel;

        [SerializeField] private Button mButton;
        [SerializeField] private TextMeshProUGUI indexText;
        
        [SerializeField] private Player forPlayer;
        [SerializeField] private int mUnitIndex;
        [SerializeField] private int amountOfItem;

        private int remainAmount;

        private void OnEnable()
        {
            changePlayerChannel.AddListener(UpdateRemainAmount);
            resetGameChannel.AddListener(ResetUnitButton);
            mButton.onClick.AddListener(WhichUnit);
        }

        private void OnDisable()
        {
            changePlayerChannel.RemoveListener(UpdateRemainAmount);
            resetGameChannel.RemoveListener(ResetUnitButton);
            mButton.onClick.RemoveListener(WhichUnit);
        }
        
        private void Start()
        {
            ResetUnitButton();
        }

        private void ResetUnitButton()
        {
            remainAmount = amountOfItem;
            indexText.text = $"{mUnitIndex} ({remainAmount})";
            mButton.interactable = true;
        }

        public void WhichUnit()
        {
            if (currentState.GetValue().NextPlayer == forPlayer)
            {
                askUnitIndex.SetValue(mUnitIndex);
            }
        }

        private void UpdateRemainAmount()
        {
            var inventory = currentState.GetValue().PlayerInventories[forPlayer];
            remainAmount = inventory.GetInventoryDictionary()[mUnitIndex];
            indexText.text = $"{mUnitIndex} ({remainAmount})";
            if (remainAmount <= 0)
            {
                mButton.interactable = false;
            }
        }
    }
}