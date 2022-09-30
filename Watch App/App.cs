using System;
using System.Linq;
using Tizen.Wearable.CircularUI.Forms;
using Xamarin.Forms;
using Samsung.Sap;
using System.Text;
using Xamarin.Forms.PlatformConfiguration.TizenSpecific;
using Xamarin.Forms.Markup;

namespace TizenNoXaml
{
    public class App : Xamarin.Forms.Application
    {
        private Agent Agent;
        private Connection Connection;
        private Peer Peer;
        private Channel ChannelId;
        public App()
        {
            MainPage = new CirclePage
            {
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.EndAndExpand,
                    HorizontalOptions = LayoutOptions.Center,
                    Children = {
                    new Button {
                        Text = "Connect",
                        Command = new Command(Connect)
                    },
                    AppButton(),
                    MessageToPhone()
                }
                }
            };
        }

        private Button AppButton()
        {
            var button=new Button
            {
                Text = "Launch Store",
                Command = new Command(DeepLinkLaunchStore)
            };
            button.On<Xamarin.Forms.PlatformConfiguration.Tizen>().SetStyle(ButtonStyle.SelectMode);
            return button;      
        }

        private Button MessageToPhone()
        {
            var button=new Button
            {
                Text = "Message to phone",
                Command = new Command(Button_Clicked_Send),
            };
            button.On<Xamarin.Forms.PlatformConfiguration.Tizen>().SetStyle(ButtonStyle.Bottom);
            return button;
        }

        private void Button_Clicked_Send()
        {
            try
            {
                if (Peer != null)
                {
                    Connection.Send(ChannelId, Encoding.UTF8.GetBytes("Hello from watch!"));
                    ShowMessage("Sent to phone");
                }
                else
                {
                    ShowMessage("Connect to phone first");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("SendMessage error: " + ex);
            }
        }

        private void DeepLinkLaunchStore()
        {
            Tizen.Applications.AppControl launchStore = new Tizen.Applications.AppControl();
            string storeUrl = @"https://play.google.com/store/apps/details?id=[PACKAGE-NAME-HERE]";
            launchStore.Operation = Tizen.Applications.AppControlOperations.Default;
            launchStore.ApplicationId = "com.samsung.w-manager-service";
            launchStore.ExtraData.Add("deeplink", storeUrl);
            launchStore.ExtraData.Add("type", "phone");

            try
            {
                Tizen.Applications.AppControl.SendLaunchRequest(launchStore);
            }
            catch (Exception e)
            {
                Console.WriteLine("Store launch error: " + e);
            }
        }
        private async void Connect()
        {
            try
            {
                Agent = await Agent.GetAgent("/example/companion");
                var peers = await Agent.FindPeers();
                ChannelId = Agent.Channels.First().Value;
                if (peers.Count() > 0)
                {
                    Console.WriteLine("Peer found");
                    Peer = peers.First();
                    Connection = Peer.Connection;
                    Connection.DataReceived -= Connection_DataReceived;
                    Connection.DataReceived += Connection_DataReceived;
                    await Connection.Open();
                    ShowMessage("Connected");
                }
                else
                {
                    ShowMessage("Peer not found1111");
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error: " + ex.Message);
            }
        }

        private void ShowMessage(string message, string debugLog = null)
        {
            Toast.DisplayText(message, 1000);
            if (debugLog != null)
            {
                debugLog = message;
            }
            Console.WriteLine("[DEBUG] " + message);
        }

        private void Connection_DataReceived(object sender, DataReceivedEventArgs e)
        {
            Toast.DisplayText("Message received");
            var message = System.Text.Encoding.ASCII.GetString(e.Data);
            Toast.DisplayText("Message received"+ message);
        }
        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
