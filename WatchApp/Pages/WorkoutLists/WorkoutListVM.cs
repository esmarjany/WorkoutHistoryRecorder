using System.Windows.Input;
using WorkoutHistoryRecorder.Contract;
using WorkoutHistoryRecorder.WatchApp.Infra;
using WorkoutHistoryRecorder.WatchApp.Pages.WorkoutLists;

namespace WorkoutHistoryRecorder.WatchApp.Pages.WorkoutList
{
    class WorkoutListVM : ViewModelBase
    {
        public WorkoutListVM(string title, int value)
        {
            Type = WorkoutListVMType.Day;
            Title = title;
            Day = value;
            ItemTapedCommand = new ItemTapedCommand(this);
        }
        public WorkoutListVM(WorkoutRecord workoutRecord, Workout workout)
        {
            Type = WorkoutListVMType.Workout;
            Title = workout.Title;
            WorkoutRecord = workoutRecord;
            ItemTapedCommand = new ItemTapedCommand(this,workoutRecord, workout);
        }
        public bool IsDone { get => Type==WorkoutListVMType.Day?false: WorkoutRecord.IsDone; }
        public WorkoutRecord WorkoutRecord { get; }
        public WorkoutListVMType Type { get; }
        public string Title { get; }
        public int Day { get; }
        //public bool IsWorkout { get => Type == WorkoutListVMType.Workout; }
        public void IsDoneChange()
        {
            OnPropertyChanged(nameof(IsDone));
        }
        public ICommand ItemTapedCommand { get; }
    }
}
