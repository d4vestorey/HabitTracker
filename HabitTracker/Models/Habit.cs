using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HabitTracker.Models
{
    internal class Habit
    {
        internal int Id { get; set; }
        internal DateTime Date { get; set; }
        internal string Description { get; set; }
        internal int Count { get; set; }

        internal Habit(DateTime date, string description, int count)
        {
            Date = date;
            Description = description;
            Count = count;
        }

        internal Habit(int id, DateTime date, string description, int count)
        {
            Id = id;
            Date = date;
            Description = description;
            Count = count;
        }
    }
}