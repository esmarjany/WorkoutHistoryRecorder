using App2.Droid.Provider;

namespace WearableCompanion.Droid
{
    public class PhoneResult
    {
        public PhoneResult(CommandType commandType, string result)
        {
            CommandType = commandType;
            Result = result;
        }

        public CommandType CommandType { get; }
        public string Result { get; }
    }
}