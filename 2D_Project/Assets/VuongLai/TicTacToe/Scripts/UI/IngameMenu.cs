using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngameMenu : MonoBehaviour
{
    [SerializeField] CanvasManager canvasManager;
    [SerializeField] Button _changePlayerButton;
    [SerializeField] TextMeshProUGUI _playerText;
    [SerializeField] PlayerInfor playerInfor;

    private void Awake()
    {
        _changePlayerButton.onClick.AddListener(OnChangePlayer);
    }

    private void OnChangePlayer()
    {
        canvasManager.RunEventChangePlayer();

        _playerText.SetText(playerInfor.IsFirstPlayer() ? "Player1" : "Player2");
    }
}
