using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutHistoryRecorder.Contract;
using WorkoutHistoryRecorder.WatchApp.Infra;
using WorkoutHistoryRecorder.WatchApp.Pages.WorkoutList;
using WorkoutHistoryRecorder.WatchApp.Pages.WorkoutRecords;
using Xamarin.Forms;

namespace WorkoutHistoryRecorder.WatchApp.Pages.WorkoutLists
{
    internal class ItemTapedCommand : AsyncCommandBase
    {
        WorkoutListVM _workoutVM;
        private WorkoutRecord _workoutRecord;
        private Workout _workout;

        public ItemTapedCommand(WorkoutListVM workoutListVM)
        {
            _workoutVM = workoutListVM;
        }

        public ItemTapedCommand(WorkoutListVM workoutListVM, WorkoutRecord workoutRecord, Workout workout)
        {
            _workoutVM = workoutListVM;
            _workoutRecord = workoutRecord;
            _workout = workout;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            try
            {
                ContentPage page;
                if (_workoutVM.Type == WorkoutListVMType.Day)
                    page = new WorkoutListPage(false, _workoutVM.WorkoutID);
                else
                    page = new WorkoutRecordPage(new WorkoutRecordVM(_workout, _workoutRecord));
                await Application.Current.MainPage.Navigation.PushModalAsync(page);
            }
            catch (Exception ex)
            {                
                MyLoger.Log(ex);
            }
        }
    }
}
