using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorkoutHistoryRecorder.PhoneApp.Pages.WorkoutTemplates
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WorkoutTemplatePage : ContentPage
    {
        public WorkoutTemplatePage()
        {
            InitializeComponent();
            BindingContext = new WorkoutTemplateVM();
        }

        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            ((WorkoutTemplateVM)BindingContext).FetchData();
        }
    }
}