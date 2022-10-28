using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Tizen.Wearable.CircularUI.Forms;
using TizenNoXaml;
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
        private readonly bool _showDays;
        private readonly int _day;

        public WorkoutListPage(bool showDays, int day = 0)
        {
            _showDays = showDays;
            _day = day;

            if (!_showDays)
                _workouts = StorageService.GetWorkouts(_day);
            Content = new StackLayout { Children = { CreateListView(_showDays) } };

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
                var wors = StorageService.GetWorkoutRecord(DateTime.Now.Date);
                var items = _workouts.Select(wo =>
                {
                    var wor = wors.FirstOrDefault(x => x.WorkoutID == wo.ID);
                    if (wor == null)
                        wor = new WorkoutRecord(Guid.NewGuid(), wo.ID, 0, DateTime.Now);

                    return new WorkoutListVM(wor, wo);
                });
                MyLoger.Log(JsonConvert.SerializeObject(items));
                res.ItemsSource = new ObservableCollection<WorkoutListVM>(items);
            }
            else
                res.ItemsSource = new[] {
                new WorkoutListVM("روز اول",1),
                new WorkoutListVM("روز دوم",2),
                new WorkoutListVM("روز سوم",3)};
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
