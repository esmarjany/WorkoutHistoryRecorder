using System;
using System.Collections.Generic;
using System.Text;

namespace WorkoutHistoryRecorder.Contract
{
    public class Workout
    {
        public Workout(Guid id, string title, int day, string amount)
        {
            ID = id;
            Title = title;
            Day = day;
            Amount = amount;
        }

        public string Title { get; }
        public string Amount { get; }
        public int Day { get; }
        public Guid ID { get; }
    }
}

