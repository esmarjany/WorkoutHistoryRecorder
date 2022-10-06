using System;
using System.Collections.Generic;
using System.Linq;
using Tizen.Wearable.CircularUI.Forms;
using WorkoutHistoryRecorder.Contract;
using WorkoutHistoryRecorder.WatchApp.Infra;
using Xamarin.Forms;

namespace WorkoutHistoryRecorder.WatchApp.Pages
{
    internal class WorkoutListPage : ContentPage
    {
        private IEnumerable<Workout> _workouts;
        private readonly bool _showDays;
        private readonly int _day;

        public WorkoutListPage(bool showDays, int day = 0)
        {
            _showDays = showDays;
            _day = day;
        }

        private CircleListView CreateListView(bool showDays)
        {
            var res = new CircleListView();
            res.ItemTapped += Res_ItemTappedAsync;

            if (!showDays)
                res.ItemsSource = _workouts.Select(c => new WorkoutListVM(c, StorageService.GetWorkoutRecord(DateTime.Now.Date).Any(x => x.WorkoutID == c.ID)));
            else
                res.ItemsSource = new[] {
                new WorkoutListVM("روز اول",1),
                new WorkoutListVM("روز دوم",2),
                new WorkoutListVM("روز سوم",3)};
            return res;
        }

        private async void Res_ItemTappedAsync(object sender, ItemTappedEventArgs e)
        {
            if (!(e.Item is WorkoutListVM workoutVM))
                return;

            ContentPage page;
            if (workoutVM.Type == WorkoutListVMType.Day)
                page = new WorkoutListPage(false, workoutVM.Day);
            else
                page = new WorkoutRecordPage(new WorkoutRecordPage.WorkoutRecordVM(workoutVM.Workout));
            await Application.Current.MainPage.Navigation.PushModalAsync(page);
        }

        protected override void OnAppearing()
        {
            if (!_showDays)
                _workouts = StorageService.GetWorkouts(_day);
            Content = new StackLayout { Children = { CreateListView(_showDays) } };
            
            base.OnAppearing();
        }
    }

    enum WorkoutListVMType { Day, Workout }
    class WorkoutListVM
    {
        public WorkoutListVM(string title, int value)
        {
            Type = WorkoutListVMType.Day;
            Title = title;
            Day = value;
        }
        public bool IsDone { get; }
        public WorkoutListVM(Workout workout, bool isDone)
        {
            Type = WorkoutListVMType.Workout;
            Title = workout.Title;
            Workout = workout;
            IsDone = isDone;
        }
        public Workout Workout { get; }

        public override string ToString()
        {
            return Title + IsDone;
        }
        public WorkoutListVMType Type { get; }
        public string Title { get; }
        public int Day { get; }
    }
}
