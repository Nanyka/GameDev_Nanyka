using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngameMenu : MonoBehaviour
{
    // [SerializeField] CanvasManager canvasManager;
    [SerializeField] private VoidChannel changePlayerChannel;
    [SerializeField] Button _changePlayerButton;
    [SerializeField] TextMeshProUGUI _playerText;
    [SerializeField] private BooleanStorage isPlayer;
    // [SerializeField] PlayerInfor playerInfor;

    private void Awake()
    {
        _changePlayerButton.onClick.AddListener(OnChangePlayer);
    }

    private void OnChangePlayer()
    {
        // canvasManager.RunEventChangePlayer();
        // playerInfor.ChangePlayer();
        isPlayer.value = !isPlayer.value;
        _playerText.SetText(isPlayer.value ? "Player1" : "Player2");

        changePlayerChannel.channel.Invoke();
    }
}
