using App2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkoutHistoryRecorder.Contract;
using WorkoutHistoryRecorder.PhoneApp.Pages.WorkoutList;
using WorkoutHistoryRecorder.PhoneApp.Pages.WorkoutTemplates;
using WorkoutHistoryRecorder.WatchApp.Infra;
using Xamarin.Forms;

namespace WorkoutHistoryRecorder.PhoneApp.Pages.WorkoutRecordList
{
    public class WorkoutRecordListVM : ViewModelBase
    {
        public WorkoutRecordListVM()
        {
            Working = new WorkoutRecord();
            SaveCommand = new Command(Save);
        }

        private IEnumerable<WorkoutRecordVM> _records;

        public IEnumerable<WorkoutRecordVM> Records
        {
            get { return _records; }
            set
            {
                _records = value;
                OnPropertyChanged();
            }
        }

        private IEnumerable<Workout> _workouts;

        public IEnumerable<Workout> Workouts
        {
            get { return _workouts; }
            set
            {
                _workouts = value;
                OnPropertyChanged(nameof(Workouts));
            }
        }
        private Workout _workout;
        public Workout SelectedWorkout
        {
            get { return _workout; }
            set
            {
                _workout = value;
                Working.WorkoutID = value==null?0:value.ID;                
            }
        }
        private async void Save()
        {
            await App.Database.SaveAsync(Working);
            Working = new WorkoutRecord();
            SelectedWorkout = new Workout();
            await FetchDataAsync();
        }

        public async Task FetchDataAsync()
        {
            var r = await App.Database.GetAllAsync<WorkoutRecord>();
            //foreach (var item in r)
            //{
            //await App.Database.DeleteAsync(item);

            //}
            Records = (await App.Database.GetAllAsync<WorkoutRecord>())
                .Select(x => new WorkoutRecordVM(x, Workouts.SingleOrDefault(y => y.ID == x.WorkoutID), this)).ToList();
        }

        private WorkoutRecord _working;
        public WorkoutRecord Working
        {
            get { return _working; }
            set
            {
                _working = value;
                SelectedWorkout = Workouts?.SingleOrDefault(x=>x.ID==_working.WorkoutID);
                OnPropertyChanged(nameof(Working));
                OnPropertyChanged(nameof(SelectedWorkout));
            }
        }

        public WorkoutTemplate WorkoutTemplate { get; }
        public ICommand SaveCommand { get; set; }

        public async Task FetChData()
        {
            //Workouts = (await App.Database.GetAllAsync<Workout>()).Where(x => x.WorkoutTemplateID == WorkoutTemplate.ID).ToList();
            Workouts = (await App.Database.GetAllAsync<Workout>()).ToList();
        }

        internal void SetWorking(WorkoutRecord working)
        {
            Working = working;
        }
    }

    public class WorkoutRecordVM
    {
        public WorkoutRecordVM(WorkoutRecord workoutRecord, Workout workout, WorkoutRecordListVM workoutRecordListVM)
        {
            Working = workoutRecord;
            Workout = workout;
            _workoutRecordListVM = workoutRecordListVM;
            DeleteCommand = new Command(Delete);
            UpdateCommand = new Command(Update);
        }
        WorkoutRecordListVM _workoutRecordListVM;
        private Workout _workout;
        public Workout Workout
        {
            get { return _workout; }
            set
            {
                _workout = value;
                Working.WorkoutID = value == null ? 0 : value.ID;
            }
        }

        public WorkoutRecord Working { get; private set; }

        public string Title
        {
            get => $"{Workout?.Title}  {Record} {Working.Date.ToShortDateString()}";
        }

        public string Record
        {
            get { return Working.Record.ToString(); }
            set
            {
                decimal.TryParse(value, out decimal temp);
                Working.Record = temp;
            }
        }
        private async void Delete()
        {
            await App.Database.DeleteAsync(Working);
            await _workoutRecordListVM.FetchDataAsync();
        }
        private void Update()
        {
            _workoutRecordListVM.SetWorking(Working);
        }


        public ICommand DeleteCommand { get; set; }
        public ICommand UpdateCommand { get; set; }
    }
}
