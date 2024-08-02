using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngameMenu : MonoBehaviour
{
    [SerializeField] Button _changePlayerButton;
    [SerializeField] TextMeshProUGUI _playerText;
    [SerializeField] private V_VoidChannel changePlayerChannel;
    [SerializeField] private V_BooleanStorage isPlayer;

    private void Awake()
    {
        _changePlayerButton.onClick.AddListener(OnChangePlayer);
    }

    private void OnChangePlayer()
    {
        isPlayer.SetValue(!isPlayer.GetValue());
        _playerText.SetText(isPlayer.GetValue() ? "Player1" : "Player2");

        changePlayerChannel.RunVoidChannel();
    }
}
