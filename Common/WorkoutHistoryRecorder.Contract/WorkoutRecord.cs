using System;
namespace WorkoutHistoryRecorder.Contract
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
        public decimal Record { get; set; }
        public DateTime Date { get; }
        public bool IsDone { get => Record != 0; }
    }
}