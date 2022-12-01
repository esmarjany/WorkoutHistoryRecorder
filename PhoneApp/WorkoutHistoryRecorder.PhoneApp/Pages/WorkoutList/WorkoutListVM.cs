using App2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using WorkoutHistoryRecorder.Contract;
using WorkoutHistoryRecorder.WatchApp.Infra;
using Xamarin.Forms;

namespace WorkoutHistoryRecorder.PhoneApp.Pages.WorkoutList
{
    internal class WorkoutListVM:ViewModelBase
    {
        public WorkoutListVM(WorkoutTemplate workoutTemplate)
        {
            WorkoutTemplate = workoutTemplate?? throw new ArgumentException();
            RefetchData();

            AddCommand = new Command(Add);
        }

        private void RefetchData()
        {
            Workouts = App.Database.GetAllAsync<Workout>().Result.Where(x=>x.WorkoutTemplateID== WorkoutTemplate.ID)
                .Select(x => new WorkoutListItemVM(x, this, WorkoutTemplate)).ToList();
        }

        public List<WorkoutListItemVM> Workouts { get; private set; }

        public ICommand AddCommand { get; set; }
        public WorkoutTemplate WorkoutTemplate { get; }

        public void Add()
        {
            App.AppMainPage.Navigation.PushModalAsync(new WorkoutPage(WorkoutTemplate));
        }

        internal void RefreshData()
        {
            RefetchData();
            OnPropertyChanged(nameof(Workouts));
        }
    }

    internal class WorkoutListItemVM: ViewModelBase
    {
        public WorkoutListItemVM(Workout workout, WorkoutListVM workoutListVM)
        {
            Workout = workout;
            WorkoutListVM = workoutListVM;
            DeleteCommand = new Command(Delete);
            UpdateCommand = new Command(Update);
        }

        public WorkoutListItemVM(Workout workout, WorkoutListVM workoutListVM, WorkoutTemplate workoutTemplate) : this(workout, workoutListVM)
        {
            WorkoutTemplate = workoutTemplate;
        }

        private async void Delete()
        {
            await App.Database.DeleteAsync(Workout);
            WorkoutListVM.RefreshData();
        }
        private async void Update()
        {
           await App.AppMainPage.Navigation.PushModalAsync(new WorkoutPage(Workout, WorkoutTemplate));
        }
        public Workout Workout { get; set; }
        public WorkoutListVM WorkoutListVM { get; }
        public ICommand DeleteCommand { get; set; }
        public ICommand UpdateCommand { get; set; }
        public WorkoutTemplate WorkoutTemplate { get; }
    }
}
