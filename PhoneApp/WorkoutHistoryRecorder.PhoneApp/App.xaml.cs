using System;
using System.IO;
using WorkoutHistoryRecorder.PhoneApp.Persistence;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
            AppMainPage = MainPage;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            System.Exception ex = (System.Exception)e.ExceptionObject;
            Console.WriteLine(ex);
        }

        static WHRDatabase database;

        public static Page AppMainPage { get; private set; }


        // Create the database connection as a singleton.
        public static WHRDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new WHRDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "WHRDatabase.db3"));
                }
                return database;
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
