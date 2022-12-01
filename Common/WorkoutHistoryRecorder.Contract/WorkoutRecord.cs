using System;
namespace WorkoutHistoryRecorder.Contract
{
    public class WorkoutRecord:EntityBase
    {
        public WorkoutRecord(int id, int workoutID, decimal record, DateTime date)
        {
            ID = id;
            WorkoutID = workoutID;
            Record = record;
            Date = date;
        }
        public WorkoutRecord()
        {

        }

        public int WorkoutID { get; set; }
        public decimal Record { get; set; }
        public DateTime Date { get; set; }
        public bool IsDone { get => Record != 0; }
    }
}