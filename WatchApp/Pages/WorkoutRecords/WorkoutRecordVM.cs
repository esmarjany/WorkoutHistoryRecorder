using System;
using System.Windows.Input;
using TizenNoXaml;
using WorkoutHistoryRecorder.Contract;
using WorkoutHistoryRecorder.WatchApp.Infra;
using Xamarin.Forms;

namespace WorkoutHistoryRecorder.WatchApp.Pages.WorkoutRecords
{
    public class WorkoutRecordVM : ViewModelBase
    {
        public WorkoutRecordVM(Workout workout, WorkoutRecord workoutRecord)
        {
            Workout = workout;
            WorkoutRecord = workoutRecord;
            SaveCommand = new WorloutRecordSaveCommand(this);
        }

        public string Record
        {
            get { return WorkoutRecord.Record.ToString(); }
            set
            {
                decimal r;
                decimal.TryParse(value, out r);
                WorkoutRecord.Record = r;
            }
        }

        public decimal RecordD
        {
            get { return WorkoutRecord.Record; }
            set
            {
                WorkoutRecord.Record = value;
                OnPropertyChanged(nameof(Record));
            }
        }
        
        public Workout Workout { get; }
        public WorkoutRecord WorkoutRecord { get; set; }
        public ICommand SaveCommand { get; set; }
    }
}
