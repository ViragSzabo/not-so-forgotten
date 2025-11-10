namespace NotSoForgottenCemetery.Features.HabitItems
{
    public class Habit(string title, string description)
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Title { get; set; } = title;
        public string Description { get; set; } = description;
        public int Streak { get; private set; } = 0;
        public DateTime? LastCompleted { get; private set; } = null;

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