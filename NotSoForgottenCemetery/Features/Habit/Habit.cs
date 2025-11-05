using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NotSoForgottenCemetery.Features
{
    public class Habit
    {
        public Guid Id { get; private set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Streak { get; private set; }
        public DateTime? LastCompleted { get; private set; }

        // Constructor
        public Habit(string title, string description)
        {
            Id = Guid.NewGuid();
            Title = title;
            Description = description;
            Streak = 0;
            LastCompleted = null;
        }

        // Method to mark the habit as completed
        public void Complete()
        {
            var today = DateTime.Today;
            if (LastCompleted == null || LastCompleted.Value.Date < today)
            {
                if (LastCompleted != null && LastCompleted.Value.Date == today.AddDays(-1))
                {
                    Streak++;
                }
                else
                {
                    Streak = 1;
                }
                LastCompleted = today;
            }
        }

        // Method to reset the habit streak
        public void ResetStreak() => Streak = 0;

        // Override ToString for better readability
        public override string ToString()
        {
            var lastCompletedStr = LastCompleted.HasValue ? LastCompleted.Value.ToShortDateString() : "Never";
            return $"{Title} - {Description} | Streak: {Streak} | Last Completed: {lastCompletedStr}";
        }
    }
}