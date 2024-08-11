using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Localization.Platform.Android;
using UnityEngine;
using UnityEngine.UI;

namespace V_TicTacToe
{
    public class IngameMenu : MonoBehaviour
    {
        [SerializeField] private Button _changePlayerButton;
        [SerializeField] private TextMeshProUGUI _playerText;
        [SerializeField] private V_VoidChannel changePlayerChannel;
        [SerializeField] private V_VoidChannel endTurnChannel;
        [SerializeField] private V_VoidChannel showIngameMenuChannel;

        [SerializeField] private V_IntegerStorage currentPlayerId;

        [SerializeField] private V_BooleanStorage isPlayed;

        private void Awake()
        {
            _changePlayerButton.onClick.AddListener(OnChangePlayer);

            _changePlayerButton.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            endTurnChannel.AddListener(OnEndTurn);
            showIngameMenuChannel.AddListener(Init);
        }

        private void OnDisable()
        {
            endTurnChannel.RemoveListener(OnEndTurn);
            showIngameMenuChannel.RemoveListener(Init);
        }

        private void Init()
        {
            _changePlayerButton.gameObject.SetActive(false);
        }

        private void OnChangePlayer()
        {
            if (currentPlayerId.GetValue().Equals(0))
            {
                currentPlayerId.SetValue(1);
                _playerText.SetText("Player2");
            }
            else if (currentPlayerId.GetValue().Equals(1))
            {
                currentPlayerId.SetValue(0);
                _playerText.SetText("Player1");
            }

            isPlayed.SetValue(false);

            _changePlayerButton.gameObject.SetActive(false);

            changePlayerChannel.RunVoidChannel();
        }

        private void OnEndTurn()
        {
            _changePlayerButton.gameObject.SetActive(true);
        }
    }
}