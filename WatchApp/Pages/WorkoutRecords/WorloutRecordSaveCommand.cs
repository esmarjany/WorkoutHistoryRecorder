using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TizenNoXaml;
using WorkoutHistoryRecorder.Contract;
using WorkoutHistoryRecorder.WatchApp.Infra;
using Xamarin.Forms;

namespace WorkoutHistoryRecorder.WatchApp.Pages.WorkoutRecords
{
    internal class WorloutRecordSaveCommand : CommandBase
    {
        WorkoutRecordVM _workoutRecord;

        public WorloutRecordSaveCommand(WorkoutRecordVM workoutRecord)
        {
            _workoutRecord = workoutRecord;
        }

        public override void Execute(object parameter)
        {
            StorageService.AddWorkoutRecord(_workoutRecord.WorkoutRecord);
            MyLoger.Log(_workoutRecord.WorkoutRecord.Record.ToString());
            Application.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}
