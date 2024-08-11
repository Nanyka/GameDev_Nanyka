using System.Collections;
using System.Collections.Generic;
using TheAiAlchemist;
using UnityEngine;

namespace V_TicTacToe
{
    public class CanvasManager : MonoBehaviour
    {
        [SerializeField] UIBase currentMenu;
        // [SerializeField] EventScriptableObject eventScriptableObject;
        [SerializeField] private VoidChannel changePlayerChannel;
        [SerializeField] PlayerInfor playerInfor;

        public void RunEventChangePlayer()
        {
            // playerInfor.ChangePlayer();

            // changePlayerChannel.channel.Invoke();
            // eventScriptableObject.RunEventChangePlayer();
        }
    }
}