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
            var cur = StorageService.GetWorkoutRecord(_workoutRecord.WorkoutRecord.ID);
            if (cur == null)
                StorageService.AddWorkoutRecord(_workoutRecord.WorkoutRecord);
            else
                StorageService.UpdateWorkoutRecord(_workoutRecord.WorkoutRecord);
            Application.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}
