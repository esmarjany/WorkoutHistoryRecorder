namespace App2.Droid.Provider
{
    public class WatchCommand
    {
        public WatchCommand(CommandType commandType, byte[] argument)
        {
            CommandType = commandType;
            Argument = argument;
        }

        public CommandType CommandType { get; }
        public byte[] Argument { get; }
        public string StringArgument { get { return System.Text.Encoding.Unicode.GetString(Argument); } }
    }

    public enum CommandType
    {
        None=0,
        GetWorkoutList=1,
        StoreRecord=2,
        Message=3
    }
}