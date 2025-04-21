using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace TheAiAlchemist
{
    public class Unit : MonoBehaviour, IUnit
    {
        [SerializeField] private VoidChannel resetGameChannel;
        [SerializeField] private IntStorage askUnitIndex;
        [SerializeField] private int mUnitIndex;
        [SerializeField] private int amountOfItem;

        private Button mButton;
        private int remainAmount;

        private void OnEnable()
        {
            resetGameChannel.AddListener(ResetUnitButton);
        }

        private void OnDisable()
        {
            resetGameChannel.RemoveListener(ResetUnitButton);
        }

        private void Start()
        {
            ResetUnitButton();
        }

        private void ResetUnitButton()
        {
            remainAmount = amountOfItem;
            mButton.interactable = true;
        }

        public int WhichUnit()
        {
            return mUnitIndex;
        }

        public void WithdrawOneItem()
        {
            
        }
    }
}