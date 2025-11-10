namespace NotSoForgottenCemetery.Features
{
    public class MoodEntry
    {
        // Properties
        public Guid Id { get; private set; }
        public DateTime Date { get; set; }
        public int MoodLevel { get; set; } // 1-10 scale
        public string Notes { get; set; }

        // Constructor
        public MoodEntry(int moodLevel, string notes)
        {
            Id = Guid.NewGuid();
            Date = DateTime.Now;
            MoodLevel = moodLevel;
            Notes = notes;
        }

        public override string ToString()
        {
            return $"Date: {Date}, Mood Level: {MoodLevel}, Notes: {Notes}";
        }

        // Method to update mood level and notes
        public void UpdateEntry(int moodLevel, string notes)
        {
            MoodLevel = moodLevel;
            Notes = notes;
        }

        // Method to get a summary of the mood entry
        public string GetSummary()
        {
            return $"On {Date.ToShortDateString()}, your mood was rated at {MoodLevel}/10. Notes: {Notes}";
        }

        // Method to check if the mood level is within a healthy range
        public string GetMoodAdvice()
        {
            if (MoodLevel >= 4 && MoodLevel <= 7)
            {
                return "Good balance! Keep it up! There are good days and bad.";
            }
            else if (MoodLevel < 4)
            {
                return "It's okay to have low days. Consider reaching out to a friend or professional if needed.";
            }
            else // MoodLevel > 7
            {
                return "Feeling great is wonderful! Just ensure you're also taking care of your mental health.";
            }
        }

        // Method to compare two mood entries
        public int CompareTo(MoodEntry other)
        {
            if (other == null) return 1;
            return this.Date.CompareTo(other.Date);
        }

        // Static method to calculate average mood level from a list of entries
        public static double CalculateAverageMood(List<MoodEntry> entries)
        {
            if (entries == null || entries.Count == 0) return 0;
            return entries.Average(e => e.MoodLevel);
        }

        // Static method to filter entries by a specific date range
        public static List<MoodEntry> FilterByDateRange(List<MoodEntry> entries, DateTime startDate, DateTime endDate)
        {
            if (entries == null) return new List<MoodEntry>();
            return entries.Where(e => e.Date >= startDate && e.Date <= endDate).ToList();
        }

        // Static method to get the highest mood entry
        public static MoodEntry GetHighestMoodEntry(List<MoodEntry> entries)
        {
            if (entries == null || entries.Count == 0) return null;
            return entries.OrderByDescending(e => e.MoodLevel).FirstOrDefault();
        }

        // Static method to get the lowest mood entry
        public static MoodEntry GetLowestMoodEntry(List<MoodEntry> entries)
        {
            if (entries == null || entries.Count == 0) return null;
            return entries.OrderBy(e => e.MoodLevel).FirstOrDefault();
        }
    }
}