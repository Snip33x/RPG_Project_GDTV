namespace RPG.Saving
{
    public interface ISaveable  //we make interface to avoid dependencies
    {
        object CaptureState();
        void RestoreState(object state);
    }
}