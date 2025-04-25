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
        [SerializeField] private IntChannel selectIndexChannel;

        [SerializeField] private Button mButton;
        [SerializeField] private Player forPlayer;
        [SerializeField] private int mUnitIndex;
        [SerializeField] private int amountOfItem;

        private ISelectionInteract interact;
        private int remainAmount;

        private void OnEnable()
        {
            changePlayerChannel.AddListener(UpdateRemainAmount);
            resetGameChannel.AddListener(ResetUnitButton);
            mButton.onClick.AddListener(WhichUnit);
            selectIndexChannel.AddListener(HighlightSelection);
        }

        private void OnDisable()
        {
            changePlayerChannel.RemoveListener(UpdateRemainAmount);
            resetGameChannel.RemoveListener(ResetUnitButton);
            mButton.onClick.RemoveListener(WhichUnit);
            selectIndexChannel.RemoveListener(HighlightSelection);
        }

        private void Awake()
        {
            interact = GetComponent<ISelectionInteract>();
        }

        private void Start()
        {
            ResetUnitButton();
        }

        private void ResetUnitButton()
        {
            remainAmount = amountOfItem; 
            interact.OnUpdateText($"{mUnitIndex} ({remainAmount})");
            mButton.interactable = true;
        }

        public void WhichUnit()
        {
            if (currentState.GetValue().NextPlayer == forPlayer)
            {
                askUnitIndex.SetValue(mUnitIndex);
                selectIndexChannel.ExecuteChannel(mUnitIndex);
            }
        }

        private void UpdateRemainAmount()
        {
            var inventory = currentState.GetValue().PlayerInventories[forPlayer];
            remainAmount = inventory.GetInventoryDictionary()[mUnitIndex];
            interact.OnUpdateText($"{mUnitIndex} ({remainAmount})");
            if (remainAmount <= 0)
            {
                mButton.interactable = false;
            }
        }
        
        private void HighlightSelection(int selectedIndex)
        {
            if (selectedIndex == mUnitIndex)
                interact.OnActivate();
            else
                interact.OnDeactivate();
        }
    }
}