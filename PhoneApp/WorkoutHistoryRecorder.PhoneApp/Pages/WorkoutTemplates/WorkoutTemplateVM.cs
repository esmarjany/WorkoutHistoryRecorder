using App2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using WorkoutHistoryRecorder.Contract;
using WorkoutHistoryRecorder.PhoneApp.Pages.WorkoutList;
using WorkoutHistoryRecorder.WatchApp.Infra;
using Xamarin.Forms;

namespace WorkoutHistoryRecorder.PhoneApp.Pages.WorkoutTemplates
{
    internal class WorkoutTemplateVM : ViewModelBase
    {
        private WorkoutTemplate _workoutTemplate;
        public WorkoutTemplate Working
        {
            get { return _workoutTemplate; }
            set
            {
                _workoutTemplate = value;
                OnPropertyChanged(nameof(Working));
            }
        }

        public IEnumerable<WorkoutTemplateItemVM> WorkoutTemplates { get; set; }
        public ICommand SaveCommand { get; set; }

        public WorkoutTemplateVM()
        {
            Working = new WorkoutTemplate();
            SaveCommand = new Command(Save);
        }


        private async void Save()
        {
            await App.Database.SaveAsync(Working);
            Working = new WorkoutTemplate();
            FetchData();
        }

        public async void FetchData()
        {
            WorkoutTemplates = (await App.Database.GetAllAsync<WorkoutTemplate>()).Select(x => new WorkoutTemplateItemVM(x, this)).ToList();
            OnPropertyChanged(nameof(WorkoutTemplates));
        }

        internal void SetWorking(WorkoutTemplate working)
        {
            Working = working ?? throw new ArgumentNullException(nameof(working));
        }
    }

    internal class WorkoutTemplateItemVM
    {
        public WorkoutTemplate Working { get; }
        private WorkoutTemplateVM WorkoutTemplateVM { get; }
        public WorkoutTemplateItemVM(WorkoutTemplate working, WorkoutTemplateVM workoutTemplateVM)
        {
            Working = working;
            DeleteCommand = new Command(Delete);
            UpdateCommand = new Command(Update);
            AddWorkoutCommand=new Command(AddWorkout);
            WorkoutTemplateVM = workoutTemplateVM;
        }
        private async void Delete()
        {
            await App.Database.DeleteAsync(Working);
            WorkoutTemplateVM.FetchData();
        }
        private void Update()
        {
            WorkoutTemplateVM.SetWorking(Working);
        }
        private void AddWorkout()
        {
            App.AppMainPage.Navigation.PushModalAsync(new WorkoutListPage(Working));
        }
        public ICommand AddWorkoutCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand UpdateCommand { get; set; }
    }
}
