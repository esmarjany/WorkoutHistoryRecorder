using ElmSharp;
using Newtonsoft.Json;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TizenNoXaml;
using WorkoutHistoryRecorder.Contract;
using WorkoutHistoryRecorder.Contract.Tools;

namespace WorkoutHistoryRecorder.WatchApp.Infra
{
    internal static class StorageService
    {
        public static void Init()
        {
            string dataPath = Tizen.Applications.Application.Current.DirectoryInfo.Data;
            _storagePath = Path.Combine(dataPath, "WorkoutHistoryRecorder.db");
            InitWorkout();
        }

        static DBContext _dbContext;
        static string _storagePath;
        private static void InitWorkout()
        {
            if (File.Exists(_storagePath))
            {
                _dbContext = MySerializer.DeserializeObject<DBContext>(File.ReadAllBytes(_storagePath));
                if (_dbContext.WorkoutRecords == null)
                    _dbContext.WorkoutRecords = new List<WorkoutRecord>();
                
            }
            if(_dbContext!=null && _dbContext.Workouts.Any())
                return;
            _dbContext = new DBContext();
            //_dbContext.Workouts = new List<Workout> {
            //new Workout(Guid.Parse("{26DBDEBD-21E9-4E79-A030-8D1ABE0ACA9C}"),"زیربغل سیمکش",1,"4*10"),
            //new Workout(Guid.Parse("{CE23F48B-F587-4AA6-8093-CF84EB8C3418}"),"شگارف هالتر",1,"4*10"),
            //new Workout(Guid.Parse("{D9A00F8E-B579-4D93-931A-06CF58B02DA1}"),"تست تمرین",2,"4*10"),
            //new Workout(Guid.Parse("{D9A00F8E-B579-4D93-931A-06CF58B02DA1}"),"زیربغل سیمکش",2,"4*10"),
            //new Workout(Guid.Parse("{C83E745C-25F8-43EF-B76A-01BCDD2F84C9}"),"زیربغل سیمکش",3,"4*10"),
            //new Workout(Guid.Parse("{9E2C0374-853C-4266-9FE6-E005C3A42F10}"),"جلوبازو سیکش",3,"4*10"),
            //new Workout(Guid.Parse("{99D93D7E-A566-40A1-A9ED-0644C80BEC65}"),"پشت بازو طناب",3,"4*10"),
            //new Workout(Guid.Parse("{6053A277-3B35-487D-B29C-9953B4418470}"),"زیربغل سیمکش",3,"4*10"),
            //};
            _dbContext.WorkoutRecords = new List<WorkoutRecord>();
            SaveChanges();
        }

        public static IEnumerable<Workout> GetWorkouts(int templateID)
        {
            return _dbContext.Workouts.Where(x => x.WorkoutTemplateID == templateID);
        }

        public static IEnumerable<WorkoutTemplate> GetWorkoutTemplates()
        {
            return _dbContext.WorkoutTemplates;
        }

        public static void AddWorkoutRecord(WorkoutRecord workoutRecord)
        {
            _dbContext.WorkoutRecords.Add(workoutRecord);
            SaveChanges();
        }

        private static async void SaveChanges()
        {
            await File.WriteAllBytesAsync(_storagePath, MySerializer.SerializeObject(_dbContext));
        }

        internal static void SetWorkout(IEnumerable<Workout> workouts)
        {
            _dbContext.Workouts = workouts.ToList();
            SaveChanges();
        }

        internal static IEnumerable<WorkoutRecord> GetWorkoutRecordByDate(DateTime date)
        {
             return _dbContext.WorkoutRecords.Where(x => x.Date.Date == date).ToList();
        }

        public static void ResetDB()
        {
            _dbContext = new DBContext();
            SaveChanges();
        }

        internal static WorkoutRecord GetWorkoutRecord(int id)
        {
            return _dbContext.WorkoutRecords.SingleOrDefault(x => x.ID == id);
        }

        internal static void UpdateWorkoutRecord(WorkoutRecord workoutRecord)
        {
            _dbContext.WorkoutRecords.Remove(_dbContext.WorkoutRecords.Single(x => x.ID == workoutRecord.ID));
            _dbContext.WorkoutRecords.Add(workoutRecord);
            SaveChanges();
        }

        internal static void SetWorkoutTemplate(IEnumerable<WorkoutTemplate> workoutTemplates)
        {
            _dbContext.WorkoutTemplates = workoutTemplates.ToList();
            SaveChanges();
        }

        internal static DateTime GetLastSync()
        {
            return _dbContext.LastSyncDateTime;
        }

        internal static void DeleteOlderThan(DateTime dateTime)
        {
            var recs = _dbContext.WorkoutRecords.Where(x => x.CreateDateTime < dateTime && x.UpdateDateTime < dateTime);
            foreach (var item in recs)
                _dbContext.WorkoutRecords.Remove(item);
        }
    }

    class DBContext
    {
        public DBContext()
        {
            Workouts = new List<Workout>();
            WorkoutRecords = new List<WorkoutRecord>();
            WorkoutTemplates = new List<WorkoutTemplate>();
        }
        public List<Workout> Workouts { get; set; }
        public List<WorkoutRecord> WorkoutRecords { get; set; }
        public List<WorkoutTemplate> WorkoutTemplates { get; set; }
        public DateTime LastSyncDateTime { get; set; }

    }
}
