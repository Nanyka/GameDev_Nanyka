using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Unity.Services.Economy;
using Unity.Services.Core;
using Unity.Services.Economy.Model;

namespace V_TicTacToe
{
    public class EconomyManager : MonoBehaviour
    {
        [SerializeField] private V_VoidChannel authenticationSuccessChannel;
        [SerializeField] private V_IntegerStorage ticTacToeScore;
        [SerializeField] private V_IntegerChannel updatePlayer1ScoreChannel;

        private string scoreId = "TICTACTOE_SCORE";

        private void OnEnable()
        {
            authenticationSuccessChannel.AddListener(Init);
        }

        private void OnDisable()
        {
            authenticationSuccessChannel.RemoveListener(Init);
        }

        private void Init()
        {
            GetTicTacToeScoreFromCloud();
        }

        private async void GetTicTacToeScoreFromCloud()
        {
            //EconomyService.Instance.Configuration.GetConfigAssignmentHash();
            await EconomyService.Instance.Configuration.SyncConfigurationAsync();

            CurrencyDefinition scoreDefinition = EconomyService.Instance.Configuration.GetCurrency(scoreId);
            PlayerBalance playerBalance = await scoreDefinition.GetPlayerBalanceAsync();

            int getValue = (int)playerBalance.Balance;
            ticTacToeScore.Value = getValue;

            updatePlayer1ScoreChannel.RunIntegerChannel(getValue);

            Debug.Log($"Score: {playerBalance.Balance}");
        }

        private async void PushTicTacToeScoreToCloud()
        {
            int currentScore = ticTacToeScore.Value;

            await EconomyService.Instance.PlayerBalances.SetBalanceAsync(scoreId, currentScore);
        }

        private void OnApplicationQuit()
        {
            PushTicTacToeScoreToCloud();
        }
    }
}
