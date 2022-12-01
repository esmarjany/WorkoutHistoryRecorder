using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkoutHistoryRecorder.Contract;

namespace WorkoutHistoryRecorder.PhoneApp.Persistence
{
    public class WHRDatabase
    {
        readonly SQLiteAsyncConnection database;

        public WHRDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<WorkoutTemplate>().Wait();
            database.CreateTableAsync<Workout>().Wait();
            database.CreateTableAsync<WorkoutRecord>().Wait();
            InitDBData();
        }

        private async void InitDBData()
        {
            var wos = await GetCountAsync<Workout>();
            var workoutTeplates = new List<WorkoutTemplate>
            {
                new WorkoutTemplate{ ID=1,Title="روز اول"},
                new WorkoutTemplate{ ID=2,Title="روز دوم"},
                new WorkoutTemplate{ ID=3,Title="روز سوم"},
            };
            var list = new List<Workout> {
            new Workout(0,"زیربغل سیمکش",1,"4*10"),
            new Workout(0,"شگارف هالتر",1,"4*10"),
            new Workout(0,"تست تمرین",2,"4*10"),
            new Workout(0,"زیربغل سیمکش",2,"4*10"),
            new Workout(0,"زیربغل سیمکش",3,"4*10"),
            new Workout(0,"جلوبازو سیکش",3,"4*10"),
            new Workout(0,"پشت بازو طناب",3,"4*10"),
            new Workout(0,"زیربغل سیمکش",3,"4*10"),
            };

            if (wos == 0)
            {
                foreach (var item in workoutTeplates)
                    await SaveAsync(item);
                foreach (var item in list)
                    await SaveAsync(item);
            }
        }

        public Task<List<T>> GetAllAsync<T>() where T : EntityBase, new()
        {
            //Get all notes.
            return database.Table<T>().ToListAsync();
        }


        public async Task<int> GetCountAsync<T>() where T : EntityBase, new()
        {
            //Get all notes.
            return (await database.Table<T>().CountAsync());
        }

        public Task<T> GetByIDAsync<T>(int id) where T : EntityBase, new()
        {
            // Get a specific note.
            return database.Table<T>()
                            .Where(i => i.ID == id)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SaveAsync<T>(T note) where T : EntityBase, new()
        {
            if (note.ID != 0)
            {
                // Update an existing note.
                return database.UpdateAsync(note);
            }
            else
            {
                // Save a new note.
                return database.InsertAsync(note);
            }
        }

        public Task<int> DeleteAsync<T>(T note) where T : EntityBase, new()
        {
            // Delete a note.
            return database.DeleteAsync(note);
        }
    }
}
