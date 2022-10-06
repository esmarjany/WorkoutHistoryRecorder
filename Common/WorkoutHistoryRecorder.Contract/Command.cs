namespace App2.Droid.Provider
{
    public class WatchCommand
    {
        public WatchCommand(CommandType commandType, string argument)
        {
            CommandType = commandType;
            Argument = argument;
        }

        public CommandType CommandType { get; }
        public string Argument { get; }
    }

    public enum CommandType
    {
        None=0,
        GetWorkoutList=1,
        StoreRecord=2,
        Message=3
    }
}