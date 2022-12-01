using App2;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using WorkoutHistoryRecorder.Contract;
using Xamarin.Forms;

namespace WorkoutHistoryRecorder.PhoneApp.Pages.Workouts
{
    internal class WorkoutVM
    {
        public WorkoutVM(WorkoutTemplate workingTemplate)
        {
            SaveCommand = new Command(Save);
            WorkingTemplate = workingTemplate;
            Working = new Workout { WorkoutTemplateID=workingTemplate.ID };
        }

        public WorkoutVM(WorkoutTemplate workingTemplate,Workout workout):this(workingTemplate)
        {
            Working = workout;
        }

        public Workout Working { get; set; }
        public WorkoutTemplate WorkingTemplate { get; set; }

        public ICommand SaveCommand { get; set; }
        private async void Save()
        {
            await App.Database.SaveAsync(Working);
            await App.AppMainPage.Navigation.PopModalAsync();
        }
    }
}
