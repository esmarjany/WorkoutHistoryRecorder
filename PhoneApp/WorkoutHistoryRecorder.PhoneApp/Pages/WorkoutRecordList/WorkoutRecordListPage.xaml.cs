using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutHistoryRecorder.Contract;
using WorkoutHistoryRecorder.PhoneApp.Pages.WorkoutRecordList;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorkoutHistoryRecorder.PhoneApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WorkoutRecordListPage : ContentPage
    {
        public WorkoutRecordListPage()
        {
            InitializeComponent();
            BindingContext = new WorkoutRecordListVM();
        }

        private async void ContentPage_Appearing(object sender, EventArgs e)
        {
          await  ((WorkoutRecordListVM)BindingContext).FetChData();
            await((WorkoutRecordListVM)BindingContext).FetchDataAsync();
        }
    }
}