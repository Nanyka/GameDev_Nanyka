using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace TheAiAlchemist
{
    public class Unit : MonoBehaviour, IUnit
    {
        [SerializeField] private VoidChannel resetGameChannel;
        [SerializeField] private IntStorage askUnitIndex;
        [SerializeField] private IntStorage currentPlayer;
        [SerializeField] private CircleChannel announceStateChanged;
        [SerializeField] private IPlayerBehaviorStorage player;

        // [SerializeField] private int mPlayer;
        [SerializeField] private int mUnitIndex;
        [SerializeField] private int amountOfItem;
        [SerializeField] private Button mButton;
        [SerializeField] private TextMeshProUGUI indexText;
        
        private int remainAmount;

        private void OnEnable()
        {
            resetGameChannel.AddListener(ResetUnitButton);
            announceStateChanged.AddListener(WithdrawOneItem);
            mButton.onClick.AddListener(WhichUnit);
        }

        private void OnDisable()
        {
            resetGameChannel.RemoveListener(ResetUnitButton);
            announceStateChanged.RemoveListener(WithdrawOneItem);
            mButton.onClick.RemoveListener(WhichUnit);
        }

        private void Start()
        {
            indexText.text = (mUnitIndex + 1).ToString();
            ResetUnitButton();
        }

        private void ResetUnitButton()
        {
            remainAmount = amountOfItem;
            mButton.interactable = true;
        }

        public void WhichUnit()
        {
            if (currentPlayer.GetValue() == player.GetValue().GetPlayerId())
            {
                askUnitIndex.SetValue(mUnitIndex);
            }
        }
        
        public void WithdrawOneItem(ICircleTrait circle)
        {
            if (circle.GetPlayerId() == player.GetValue().GetPlayerId() & circle.GetPriority() == mUnitIndex)
            {
                remainAmount -= 1;
                if (remainAmount <= 0)
                {
                    mButton.interactable = false;
                }
            }
        }
    }
}