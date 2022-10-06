using Android.App;
using Android.Content;
using App2;
using Com.Samsung.Android.Sdk.Accessory;
using Java.Interop;

namespace WearableCompanion.Droid
{
    public partial class ProviderService
    {
        // the custome connection between watch and android
        public class ProviderServiceSocket : SASocket
        {
            [Export(SuperArgumentsString = "\"ProviderServiceSocket\"")]
            public ProviderServiceSocket() : base(p0: "ProviderServiceSocket")
            {

            }

            public delegate void DataRecived(byte[] bytes);
            public event DataRecived OnDataRecived =delegate { };
            
            public override void OnReceive(int channelId, byte[] bytes)
            {
                OnDataRecived(bytes);                
            }

            protected override void OnServiceConnectionLost(int p0)
            {
                // ResetCache();
                Close();
                MainPage.StaticConnection = "No connection to watch";
                Intent serviceIntent = new Intent(Application.Context, typeof(ProviderService));
                Application.Context.StopService(serviceIntent);
            }

            public override void OnError(int p0, string p1, int p2)
            {
                // Error handling
            }
        }
    }
}