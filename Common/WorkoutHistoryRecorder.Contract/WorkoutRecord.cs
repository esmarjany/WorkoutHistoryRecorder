using System;
using WorkoutHistoryRecorder.Contract;

namespace WearableCompanion.Droid
{
    public class WorkoutRecord
    {
        public WorkoutRecord(Guid id, Guid workoutID, decimal record, DateTime date)
        {
            ID = id;
            WorkoutID = workoutID;
            Record = record;
            Date = date;
        }

        public Guid ID { get; }
        public Guid WorkoutID { get; }
        public decimal Record { get; }
        public DateTime Date { get;  }
    }
}