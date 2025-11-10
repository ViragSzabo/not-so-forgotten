using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NotSoForgottenCemetery.Features
{
    public class MoodTracker
    {
        // Properties
        private readonly List<MoodEntry> _entries = new List<MoodEntry>();
        public int Count => _entries.Count;

        // Method to add a new mood entry
        public void AddEntry(MoodEntry entry)
        {
            if (entry == null) throw new ArgumentNullException(nameof(entry));
            _entries.Add(entry);
        }

        // Method to get all mood entries
        public List<MoodEntry> GetAllEntries()
        {
            return _entries.OrderBy(e => e.Date).ToList();
        }

        // Method to get average mood level
        public double GetAverageMood()
        {
            if (_entries.Count == 0) return 0;
            return _entries.Average(e => e.MoodLevel);
        }

        // Method to get entries within a specific date range
        public List<MoodEntry> GetEntriesByDateRange(DateTime startDate, DateTime endDate)
        {
            return _entries.Where(e => e.Date >= startDate && e.Date <= endDate).OrderBy(e => e.Date).ToList();
        }

        // Method to clear all entries
        public void ClearEntries()
        {
            _entries.Clear();
        }

        internal void LogMood(MoodEntry moodEntry)
        {
            throw new NotImplementedException();
        }

        internal List<MoodEntry> GetMoodHistory(DateTime from, DateTime to)
        {
            throw new NotImplementedException();
        }

        internal void ClearMoods()
        {
            throw new NotImplementedException();
        }

        internal void DisplayMoods()
        {
            throw new NotImplementedException();
        }
    }
}