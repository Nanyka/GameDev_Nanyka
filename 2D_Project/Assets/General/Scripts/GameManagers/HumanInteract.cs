using AlphaZeroAlgorithm;
using UnityEngine;

namespace TheAiAlchemist
{
    public class HumanInteract : MonoBehaviour, IUnitPlacer
    {
        [SerializeField] private Vector3Channel mousePosChannel;
        [SerializeField] private IntStorage askUnitIndex;
        [SerializeField] private MoveChannel humanMoveChannel;

        private void OnEnable()
        {
            mousePosChannel.AddListener(ListenMousePos);
        }

        private void OnDisable()
        {
            mousePosChannel.RemoveListener(ListenMousePos);
        }

        public void Init(IPlayerBehavior player)
        {
            throw new System.NotImplementedException();
        }

        public void ListenMousePos(Vector3 mousePos)
        {
            if (askUnitIndex.GetValue() < 0)
            {
                Debug.Log("Need to select a unit");
            }
            else
            {
                // TODO: Check coordinators of pieces
                var humanMove = new Move(new Point( Mathf.RoundToInt(mousePos.x) + 2, 
                    Mathf.RoundToInt(mousePos.y) + 2,
                    askUnitIndex.GetValue()));
                humanMoveChannel.ExecuteChannel(humanMove);
                // Debug.Log($"Point to {humanMove}");
            }
        }
    }
}