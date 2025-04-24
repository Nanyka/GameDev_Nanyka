using AlphaZeroAlgorithm;
using UnityEngine;
using UnityEngine.Serialization;

namespace TheAiAlchemist
{
    public class HumanInteract : MonoBehaviour
    {
        [SerializeField] private PointChannel selectPointChannel;
        [SerializeField] private MoveChannel humanMoveChannel;

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