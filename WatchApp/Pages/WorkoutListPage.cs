using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tizen.Wearable.CircularUI.Forms;
using Xamarin.Forms;

namespace WorkoutHistoryRecorder.WatchApp.Pages
{
    internal class WorkoutListPage : ContentPage
    {
        public WorkoutListPage()
        {
            Content = new StackLayout
            {
                Children =
                { 
                    CreateListView()
                }
            };
        }

        private static CircleListView CreateListView()
        {
            var list=new CircleListView()
            {
                ItemsSource = new[] {"Test!","Test2" }
            };
            return list;
        }
    }
}
