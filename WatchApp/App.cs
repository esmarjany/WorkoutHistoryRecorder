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
using WorkoutHistoryRecorder.WatchApp.Pages.WorkoutList;

namespace TizenNoXaml
{
    public class App : Xamarin.Forms.Application
    {
        SyncDataService SyncDataService;
        public App()
        {
            MyLoger.Log("App started!");
            SyncDataService = new SyncDataService();
            try
            {
                StorageService.Init();
                MainPage = new CirclePage
                {
                    Content = new StackLayout
                    {
                        VerticalOptions = LayoutOptions.EndAndExpand,
                        HorizontalOptions = LayoutOptions.Center,
                        Children = {
                    new Button {
                        Text = "Connect",
                        Command = new Command(SyncDataService.Connect)
                    },
                    new Button {
                        Text = "Get Workout",
                        Command = new Command(()=>{SyncDataService.ReciveWorkout(); })
                    },
                    AppButton(),
                    ClearFileByutton(),
                    ListButton()
                }
                    }
                };
            }
            catch (Exception ex)
            {
                MyLoger.Log(ex);
            }
            MainPage.Appearing += MainPage_Appearing;
            MyLoger.Log("App End!");
        }

        private void MainPage_Appearing(object sender, EventArgs e)
        {
            try
            {
               // SyncDataService.SyncReords();
            }
            catch (Exception ex)
            {
                MyLoger.Log(ex);
            }
            MyLoger.Log("Appearing!");
        }


        private View ClearFileByutton()
        {
            return new Button
            {
                Text = "Reset DB",
                Command = new Command(() => { StorageService.ResetDB(); Toast.DisplayText("Reseted !!!"); })
            };
        }

        private Button ListButton()
        {
            var button = new Button { Text = "List" };
            button.On<Xamarin.Forms.PlatformConfiguration.Tizen>().SetStyle(ButtonStyle.Bottom);
            button.Clicked += async (sender, e) =>
            {
                await ShowList();
            };
            return button;
        }

        private Button AppButton()
        {
            var button = new Button { Text = "Launch Store", Command = new Command(DeepLinkLaunchStore) };
            return button;
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

        private async Task ShowList()
        {
            try
            {
                var PAGE = new WorkoutListPage(true);
                await MainPage.Navigation.PushModalAsync(PAGE);
            }
            catch (Exception ex)
            {
                MyLoger.Log(ex.Message);
                MyLoger.Log(ex.StackTrace);
                throw;
            }
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
