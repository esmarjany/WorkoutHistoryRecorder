using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutHistoryRecorder.PhoneApp.Pages;
using WorkoutHistoryRecorder.PhoneApp.Pages.WorkoutTemplates;
using Xamarin.Forms;

namespace App2
{
    public partial class MainPage : ContentPage
    {
        public IProviderService provider { get; set; }
        public MainPage()
        {
            provider = DependencyService.Get<IProviderService>();
            InitializeComponent();
            BindingContext = this;
        }

        private void Button_Clicked_Close(object sender, EventArgs e)
        {
            provider.CloseConnection();
        }

        private string entryString;
        public string EntryString
        {
            get => entryString;
            set
            {
                entryString = value;
                OnPropertyChanged();
            }
        }

        private void Button_Clicked_Send(object sender, EventArgs e)
        {
            provider.SendData(EntryString);
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new WorkoutTemplatePage());
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new WorkoutRecordListPage());
        }

        private void Button_Clicked_2(object sender, EventArgs e)
        {
            //Navigation.PushModalAsync(new WorkoutListPage());
        }
    }
}

