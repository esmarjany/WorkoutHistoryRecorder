using App2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutHistoryRecorder.Contract;
using WorkoutHistoryRecorder.PhoneApp.Pages.WorkoutList;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorkoutHistoryRecorder.PhoneApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WorkoutListPage : ContentPage
    {
        public WorkoutListPage(WorkoutTemplate workoutTemplate)
        {
            InitializeComponent();
            WorkoutTemplate = workoutTemplate;
        }

        public WorkoutTemplate WorkoutTemplate { get; }

        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            BindingContext = new WorkoutListVM(WorkoutTemplate);
        }
    }
}