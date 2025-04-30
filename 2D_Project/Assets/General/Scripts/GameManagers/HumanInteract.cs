using AlphaZeroAlgorithm;
using UnityEngine;
using UnityEngine.Serialization;

namespace TheAiAlchemist
{
    public class HumanInteract : MonoBehaviour
    {
        [SerializeField] private PointChannel selectPointChannel;
        [SerializeField] private MoveChannel humanMoveChannel;
        [SerializeField] private GameStateStorage gameStateStorage;

        private void OnEnable()
        {
            selectPointChannel.AddListener(ListenPoint);
        }

        private void OnDisable()
        {
            selectPointChannel.RemoveListener(ListenPoint);
        }

        private void ListenPoint(Point selectedPoint)
        {
            var checkMove = gameStateStorage.GetValue().IsValidMove(new Move(selectedPoint));
            if (!checkMove) return;
            var humanMove = new Move(selectedPoint);
            humanMoveChannel.ExecuteChannel(humanMove);

            // if (askUnitIndex.GetValue() < 0)
            // {
            //     Debug.Log("Need to select a unit");
            // }
            // else
            // {
            //     var humanMove = new Move(selectedPoint);
            //     humanMoveChannel.ExecuteChannel(humanMove);
            //     // Debug.Log($"Point to {humanMove}");
            // }
        }
    }
}