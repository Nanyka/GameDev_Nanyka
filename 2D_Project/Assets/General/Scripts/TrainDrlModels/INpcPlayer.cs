using Unity.MLAgents.Actuators;

namespace TheAiAlchemist
{
    public interface INpcPlayer
    {
        public void TakeAction(ActionSegment<int> action);
        public int GetCurrentAction();
        public int GetCurrentPriority();
        public void AskForAction();
    }
}