using System;
using System.Collections.Generic;
using System.Text;

namespace WorkoutHistoryRecorder.Contract
{
    public class Workout: EntityBase
    {
        public Workout(int id, string title, int workoutTemplateID, string amount)
        {
            ID = id;
            Title = title;
            WorkoutTemplateID = workoutTemplateID;
            Amount = amount;
        }
        public Workout()
        {

        }

    
        public string Title { get; set; }
        public string Amount { get; set; }
        public int WorkoutTemplateID { get; set; }
    }


    public class WorkoutMessage
    {
        //public IEnumerable<Workout> Workouts { get; set; }
        public IEnumerable<WorkoutTemplate> WorkoutTemplates { get; set; }
    }
}

