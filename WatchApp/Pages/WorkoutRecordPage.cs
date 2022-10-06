using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tizen.Wearable.CircularUI.Forms;
using TizenNoXaml;
using WearableCompanion.Droid;
using WorkoutHistoryRecorder.Contract;
using WorkoutHistoryRecorder.WatchApp.Infra;
using Xamarin.Forms;

namespace WorkoutHistoryRecorder.WatchApp.Pages
{
    internal class WorkoutRecordPage : ContentPage
    {
        private WorkoutRecordVM _value;
        PopupEntry _recordEntry;
        public WorkoutRecordPage(WorkoutRecordVM value)
        {
            _value = value;
            _recordEntry = new PopupEntry()
            {
                Keyboard = Keyboard.Numeric,
                ClearButtonVisibility = ClearButtonVisibility.WhileEditing,
                ReturnType = ReturnType.Done
            };

            Content = new StackLayout
            {
                Children = {
                    new Label{Text = _value.Workout.Title, HorizontalTextAlignment= TextAlignment.Center},
                    new Label{ Text= _value.Workout.Amount, HorizontalTextAlignment= TextAlignment.Center},
                    _recordEntry,
                    new Button() { Text="Save" ,Command=new Command(()=>{
                    MyLoger.Log("SaveClicked!!");
                        StorageService.AddWorkoutRecord(new WorkoutRecord(Guid.NewGuid(),_value.Workout.ID,decimal.Parse(_recordEntry.Text),DateTime.Now));
                    MyLoger.Log("Added!!");
                        Application.Current.MainPage.Navigation.PopModalAsync();
                    MyLoger.Log("Pushed!!"); 
                    }) }
            },
                VerticalOptions = LayoutOptions.EndAndExpand,
                HorizontalOptions = LayoutOptions.Center,
            };


        }

        public class WorkoutRecordVM
        {
            public WorkoutRecordVM(Workout workout)
            {
                Workout = workout;
            }

            public decimal Record { get; set; }
            public Workout Workout { get; }
        }
    }
}
