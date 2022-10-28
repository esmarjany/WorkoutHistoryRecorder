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

namespace WorkoutHistoryRecorder.WatchApp.Pages.WorkoutRecords
{
    internal partial class WorkoutRecordPage : ContentPage
    {
        public WorkoutRecordPage(WorkoutRecordVM value)
        {
            BindingContext = value;

            var txtRecord = new PopupEntry()
            {
                Keyboard = Keyboard.Numeric,
                ClearButtonVisibility = ClearButtonVisibility.WhileEditing,
                ReturnType = ReturnType.Done
            };
            txtRecord.SetBinding(Entry.TextProperty,"Record");

            var btnsave = new Button { Text = "Save" };
            btnsave.SetBinding(Button.CommandProperty, "SaveCommand");

            var layout = new StackLayout
            {
                Children = {
                    new Label{ Text = value.Workout.Title,  HorizontalTextAlignment= TextAlignment.Center},
                    new Label{ Text = value.Workout.Amount, HorizontalTextAlignment= TextAlignment.Center},
                    txtRecord,
                    btnsave
                    }
            ,
                VerticalOptions = LayoutOptions.EndAndExpand,
                HorizontalOptions = LayoutOptions.Center,
            };
            Content = layout;
        }
    }
}