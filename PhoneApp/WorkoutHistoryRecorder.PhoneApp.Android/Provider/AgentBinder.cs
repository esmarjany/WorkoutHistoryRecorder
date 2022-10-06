using Android.OS;

namespace WearableCompanion.Droid
{
    public partial class ProviderService
    {
        public class AgentBinder : Binder
        {
            public AgentBinder(ProviderService service) => Service = service;

            public ProviderService Service { get; private set; }
        }
    }
}