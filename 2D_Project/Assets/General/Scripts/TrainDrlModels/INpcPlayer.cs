namespace TheAiAlchemist
{
    public interface INpcPlayer
    {
        public void TakeAction(int action);
        public int GetCurrentAction();
    }
}