using Tizen.Pims.Contacts.ContactsViews;
using Tizen.Wearable.CircularUI.Forms;
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
                Keyboard = Keyboard.Telephone,
                //ClearButtonVisibility = ClearButtonVisibility.WhileEditing,
                //ReturnType = ReturnType.Done
            };
            //txtRecord.SetBinding(Entry.TextProperty, "Record");

            var steper = new CircleStepper
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Increment = 0.25,
                LabelFormat = "N2",
                MarkerColor = Color.Coral,
                Maximum =30,
                Minimum =0,
                Margin = new Thickness(0,0,0,40),
                
            };
            steper.SetBinding(CircleStepper.ValueProperty, "RecordD");
            
            var btnsave = new Button { Text = "Save" };
            btnsave.SetBinding(Button.CommandProperty, "SaveCommand");
            var rlbale = new Label { Text = value.Workout.Title, HorizontalTextAlignment = TextAlignment.Center };
            rlbale.SetBinding(Label.TextProperty,"Record");
            
            var layout = new StackLayout
            {
                Children = {
                    new Label{ Text = value.Workout.Title,  HorizontalTextAlignment= TextAlignment.Center},
                    new Label{ Text = value.Workout.Amount, HorizontalTextAlignment= TextAlignment.Center},
                    steper,
                   // txtRecord,
                   //rlbale,
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