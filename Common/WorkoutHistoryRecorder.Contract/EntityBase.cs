using SQLite;
using System;

namespace WorkoutHistoryRecorder.Contract
{
    public class EntityBase
    {
        public EntityBase()
        {
            CreateDateTime = DateTime.Now;
        }

        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is EntityBase entity)
                return entity.ID == ID;
            return false;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }
    }
}

