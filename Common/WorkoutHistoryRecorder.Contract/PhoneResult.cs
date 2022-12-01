using App2.Droid.Provider;

namespace WearableCompanion.Droid
{
    public class PhoneResult
    {
        public PhoneResult(CommandType commandType, byte[] result)
        {
            CommandType = commandType;
            Result = result;
        }

        public CommandType CommandType { get; }
        public byte[] Result { get; }
    }
}