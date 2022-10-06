using System;
using System.Linq;
using Tizen.Wearable.CircularUI.Forms;
using Xamarin.Forms;
using Samsung.Sap;
using System.Text;
using Xamarin.Forms.PlatformConfiguration.TizenSpecific;
using Xamarin.Forms.Markup;
using WorkoutHistoryRecorder.WatchApp.Pages;
using WorkoutHistoryRecorder.WatchApp;
using Newtonsoft.Json;
using WearableCompanion.Droid;
using App2.Droid.Provider;
using System.Collections.Generic;
using Tizen;
using Tizen.Messaging.Messages;
using System.IO;
using WorkoutHistoryRecorder.WatchApp.Infra;
using WorkoutHistoryRecorder.Contract;
using System.Threading.Tasks;

namespace TizenNoXaml
{
    public class App : Xamarin.Forms.Application
    {
        public App()
        {
            MyLoger.Log("App started!");
            _SAPService = new SAPService();         
            _SAPService.DataReceived += Connection_DataReceived;
            StorageService.Init();
            string dataPath = Tizen.Applications.Application.Current.DirectoryInfo.Data;
            var filePath = Path.Combine(dataPath, "myFile.txt");
            File.WriteAllText(filePath, "Salam");
            var res = File.ReadAllText(filePath);
            Toast.DisplayText(res);
            Toast.DisplayText(filePath);

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
                    MessageToPhone(),
                    ListButton()
                }
                }
            };
        }
        SAPService _SAPService;
        private Button ListButton()
        {
            var button = new Button { Text = "List" };
            button.Clicked += (sender, e) =>
            {
                ShowList();// _SAPService.SendText(JsonConvert.SerializeObject(new WatchCommand(CommandType.GetWorkoutList, "")));
            };
            return button;
        }


        private Button MessageToPhone()
        {
            var button = new Button { Text = "Message to phone", Command = new Command(Button_Clicked_Send), };
            button.On<Xamarin.Forms.PlatformConfiguration.Tizen>().SetStyle(ButtonStyle.Bottom);
            return button;
        }

        private async void Connect()
        {
            if (await _SAPService.Connect())
                Toast.DisplayText("Connected");
            else
                Toast.DisplayText("Connection failed!");
        }
        private Button AppButton()
        {
            var button = new Button { Text = "Launch Store", Command = new Command(DeepLinkLaunchStore) };
            button.On<Xamarin.Forms.PlatformConfiguration.Tizen>().SetStyle(ButtonStyle.SelectMode);
            return button;
        }

        private void Button_Clicked_Send()
        {
            try
            {
                _SAPService.SendText("Hello from watch!");
                ShowMessage("Sent to phone");

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
            var message = Encoding.Unicode.GetString(e.Data);
            var res = JsonConvert.DeserializeObject<PhoneResult>(message);
            if (res != null)
            {
                switch (res.CommandType)
                {
                    case CommandType.None:
                        break;
                    case CommandType.GetWorkoutList:
                        ListRecived(res);
                        break;
                    case CommandType.StoreRecord:
                        break;
                    case CommandType.Message:
                        break;
                    default:
                        break;
                }
            }

        }
        private async void ListRecived(PhoneResult res)
        {
            var wworkouts = JsonConvert.DeserializeObject<List<Workout>>(res.Result);
            StorageService.SetWorkout(wworkouts);
            await ShowList();
        }

        private async Task ShowList()
        {
            var PAGE = new WorkoutListPage(true);
            await MainPage.Navigation.PushModalAsync(PAGE);
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

    class MyLoger
    {
        public static void Log(string content)
        {
            Tizen.Log.Info("MyApp", content);
        }
    }
}
