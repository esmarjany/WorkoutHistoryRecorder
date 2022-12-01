using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using Tizen.Wearable.CircularUI.Forms;
using WorkoutHistoryRecorder.Contract;
using WorkoutHistoryRecorder.WatchApp.Infra;
using WorkoutHistoryRecorder.WatchApp.Pages.WorkoutRecords;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.Tizen;

namespace WorkoutHistoryRecorder.WatchApp.Pages.WorkoutList
{
    internal class WorkoutListPage : ContentPage
    {
        private IEnumerable<Workout> _workouts;
        private readonly bool _showTemplate;
        private readonly int _templateID;

        public WorkoutListPage(bool showTemplate, int templateID = 0)
        {
            _showTemplate = showTemplate;
            _templateID = templateID;

            if (!_showTemplate)
                _workouts = StorageService.GetWorkouts(_templateID);
            Content = new StackLayout { Children = { CreateListView(_showTemplate) } };

        }
        CircleListView res;
        private CircleListView CreateListView(bool showDays)
        {
            res = new CircleListView();
            MyLoger.Log("34");
            var dataTemplate = new DataTemplate(() =>
            {
                var baseLayout = new StackLayout()
                {
                    Orientation = StackOrientation.Horizontal,
                    FlowDirection = FlowDirection.RightToLeft,
                    VerticalOptions = LayoutOptions.Fill,
                    HorizontalOptions = LayoutOptions.Fill,
                };

                var chkIsDone = new CheckBox { IsEnabled = false };
                var imgIsDone = new Image
                {
                    Aspect = Aspect.AspectFill,
                    Source = "Done.png",
                    Margin = new Thickness(0, 10, 20, 10),
                };
                var lblTitle = new Label
                {
                    VerticalTextAlignment = TextAlignment.Center,
                    HorizontalTextAlignment = TextAlignment.Center,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    FontSize = 50,
                    HeightRequest = 80
                };

                imgIsDone.SetBinding(IsVisibleProperty, "IsDone");
                lblTitle.SetBinding(Label.TextProperty, "Title");

                baseLayout.Children.Add(imgIsDone);
                baseLayout.Children.Add(lblTitle);

                return new ViewCell { View = baseLayout };
            });

            res.ItemTemplate = dataTemplate;
            res.ItemTapped += Res_ItemTappedAsync;

            if (!showDays)
            {
                var wors = StorageService.GetWorkoutRecordByDate(DateTime.Now.Date);
                var items = _workouts.Select(wo =>
                {
                    var wor = wors.SingleOrDefault(x => x.WorkoutID == wo.ID);
                    if (wor == null)
                        wor = new WorkoutRecord(0, wo.ID, 0, DateTime.Now);

                    return new WorkoutListVM(wor, wo);
                });
                MyLoger.Log(JsonConvert.SerializeObject(items));
                res.ItemsSource = new ObservableCollection<WorkoutListVM>(items);
            }
            else
            {
                var workoutTemplates = StorageService.GetWorkoutTemplates();
                res.ItemsSource = workoutTemplates.Select(x => new WorkoutListVM(x.Title, x.ID));                
            }
            return res;
        }

        private void Res_ItemTappedAsync(object sender, ItemTappedEventArgs e)
        {
            if (!(e.Item is WorkoutListVM wo))
                return;
            try
            {
            MyLoger.Log((wo==null).ToString() );
            MyLoger.Log((wo.ItemTapedCommand==null).ToString());
                wo.ItemTapedCommand.Execute(null);
            }
            catch (Exception ex)
            {
                MyLoger.Log(ex);                
            }

        }

        protected override void OnAppearing()
        {
            res.ItemsSource.Cast<WorkoutListVM>().ForEach(x => x.IsDoneChange());
            base.OnAppearing();
        }
    }

    enum WorkoutListVMType { Day, Workout }
}
