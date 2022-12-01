using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutHistoryRecorder.Contract;
using WorkoutHistoryRecorder.PhoneApp.Pages.Workouts;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorkoutHistoryRecorder.PhoneApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WorkoutPage : ContentPage
    {
      
        public WorkoutPage( WorkoutTemplate workoutTemplate)
        {
            InitializeComponent();
            BindingContext = new WorkoutVM(workoutTemplate);
        }

        public WorkoutPage(Workout workout,WorkoutTemplate workoutTemplate):this(workoutTemplate)
        {
            BindingContext = new WorkoutVM(workoutTemplate, workout);
        }
    }
}