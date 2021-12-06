namespace RPG.Saving
{
    public interface ISaveable  //we make interface to avoid dependencies //allows us to implement saving system to any component
    {
        object CaptureState();
        void RestoreState(object state);
    }
}