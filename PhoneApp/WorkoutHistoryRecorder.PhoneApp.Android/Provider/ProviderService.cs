using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using AndroidX.Core.App;
using App2;
using App2.Droid;
using Com.Samsung.Android.Sdk;
using Com.Samsung.Android.Sdk.Accessory;
using Java.Interop;
// https://docs.microsoft.com/en-us/xamarin/android/platform/binding-java-library/
// https://xamarinhelp.com/creating-xamarin-android-binding-library/


[assembly: Xamarin.Forms.Dependency(typeof(WearableCompanion.Droid.ProviderService))]
namespace WearableCompanion.Droid
{
    [Service(Exported = true, Name = "WearableCompanion.Droid.ProviderService")]
    public partial class ProviderService : SAAgent, IProviderService
    {
        public static readonly string TAG = typeof(ProviderService).Name;
        public static IBinder mBinder { get; private set; }
        public static readonly Java.Lang.Class SASOCKET_CLASS = Java.Lang.Class.FromType(typeof(ProviderServiceSocket)).Class;
        public static ProviderServiceSocket SocketServiceProvider;
        NotificationManager manager;
        private static readonly int CHANNEL_ID = 104;
        const int pendingIntentId = 0;
        const string channelId = "sample_channel_01";
        const string channelName = "Accessory_SDK_Sample";
        bool channelInitialized = false;

        [Export(SuperArgumentsString = "\"ProviderService\", ProviderService_ProviderServiceSocket.class")]
        public ProviderService() : base("ProviderService", SASOCKET_CLASS)
        {

        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }

        // Explanation of whats happening, Bound Service
        // https://docs.microsoft.com/en-us/xamarin/android/app-fundamentals/services/creating-a-service/bound-services
        public override IBinder OnBind(Intent intent)
        {
            // This method must always be implemented
            Android.Util.Log.Debug(TAG, "OnBind");
            mBinder = new AgentBinder(this);
            return mBinder;
        }

        public override bool OnUnbind(Intent intent)
        {
            // This method is optional to implement
            Android.Util.Log.Debug(TAG, "OnUnbind");
            return base.OnUnbind(intent);
        }

        [return: GeneratedEnum]
        public override void OnStart(Intent intent, int startId)
        {
            FindPeerAgents();
            base.OnStart(intent, startId);
        }

        public override void OnCreate()
        {
            base.OnCreate();

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
                CreateNotification();

            var mAccessory = new SA();
            try
            {
                mAccessory.Initialize(this);
            }
            catch (SsdkUnsupportedException)
            {
                // try to handle SsdkUnsupportedException
            }
            catch (Exception e1)
            {
                e1.ToString();

                /*
                * Your application can not use Samsung Accessory SDK. Your application should work smoothly
                * without using this SDK, or you may want to notify user and close your application gracefully
                * (release resources, stop Service threads, close UI thread, etc.)
                */
                StopSelf();
            }


            bool isFeatureEnabled = mAccessory.IsFeatureEnabled(SA.DeviceAccessory);

        }

        private void CreateNotification()
        {
            if (!channelInitialized)
            {
                CreateNotificationChannel();
            }

            int notifyID = 1;

            Intent intent = new Intent(Android.App.Application.Context, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.NewTask | ActivityFlags.SingleTop);

            PendingIntent pendingIntent = PendingIntent.GetActivity(Android.App.Application.Context, pendingIntentId, intent, PendingIntentFlags.Immutable);

            NotificationCompat.Builder builder = new NotificationCompat.Builder(Android.App.Application.Context, channelId)
                .SetAutoCancel(true)
                .SetContentTitle("Wearable")
                .SetContentText("Connected to your watch")
                .SetContentIntent(pendingIntent)
                .SetSmallIcon(Resource.Drawable.abc_btn_check_to_on_mtrl_000)
                .SetDefaults((int)NotificationDefaults.Sound | (int)NotificationDefaults.Vibrate)
                .SetChannelId(channelId)
                .SetPriority(1)
                .SetVisibility(1)
                .SetCategory(Android.App.Notification.CategoryService);

            StartForeground(notifyID, builder.Build());
        }

        void CreateNotificationChannel()
        {
            manager = (NotificationManager)Android.App.Application.Context.GetSystemService(NotificationService);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channelNameJava = new Java.Lang.String(channelName);
                var channel = new NotificationChannel(channelId, channelNameJava, NotificationImportance.Default)
                {
                    Description = "Description test"
                };
                manager.CreateNotificationChannel(channel);
            }
            channelInitialized = true;
        }

        public void SendData(string msg)
        {
            if (SocketServiceProvider != null)
            {
                try
                {
                    SocketServiceProvider.Send(CHANNEL_ID, System.Text.Encoding.Unicode.GetBytes(msg));
                }
                catch (Exception e)
                {
                    Console.WriteLine("SendData error: " + e);
                }
            }
        }
        public void SendData(byte[] msg)
        {
            if (SocketServiceProvider != null)
            {
                try
                {
                    SocketServiceProvider.Send(CHANNEL_ID, msg);
                }
                catch (Exception e)
                {
                    Console.WriteLine("SendData error: " + e);
                }
            }
        }
        protected override void OnFindPeerAgentsResponse(SAPeerAgent[] p0, int result)
        {
#if DEBUG
            Console.WriteLine(TAG, "onFindPeerAgentResponse : result =" + result);
#endif

            if (result == PeerAgentFound)
            {
                foreach (SAPeerAgent peerAgent in p0)
                {
                    //  Cache(peerAgent);
                    RequestServiceConnection(peerAgent);

                }
            }
        }

        protected override void OnServiceConnectionRequested(SAPeerAgent p0)
        {
            if (p0 != null)
                AcceptServiceConnectionRequest(p0);
        }

        // this is where the connection socket is called
        protected override void OnServiceConnectionResponse(SAPeerAgent p0, SASocket socket, int result)
        {
            // Cache(socket);
            if (result == ConnectionSuccess)
            {
                if (socket != null)
                {
                    SocketServiceProvider = (ProviderServiceSocket)socket;
                    SocketServiceProvider.OnDataRecived += MSocketServiceProvider_OnDataRecived;
                }

            }
            else if ((result == SAAgent.ConnectionAlreadyExist))
            {
#if DEBUG
                Console.WriteLine("onServiceConnectionResponse, CONNECTION_ALREADY_EXIST");
#endif
            }
        }

        protected virtual void MSocketServiceProvider_OnDataRecived(byte[] bytes)
        {

        }

        public bool CloseConnection()
        {
            if ((SocketServiceProvider != null))
            {
                SocketServiceProvider.Close();
                SocketServiceProvider = null;

                Intent serviceIntent = new Intent(Android.App.Application.Context, typeof(ProviderService));
                Android.App.Application.Context.StopService(serviceIntent);

                return true;
            }
            else
            {
                return false;
            }

        }
    }
}