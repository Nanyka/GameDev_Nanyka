namespace TheAiAlchemist
{
    public interface ICheckState
    {
        public bool IsActivated();
        public void ChangeState();
    }
}