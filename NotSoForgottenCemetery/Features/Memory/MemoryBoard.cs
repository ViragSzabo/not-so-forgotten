using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NotSoForgottenCemetery.Features
{
    public class MemoryBoard
    {
        // Internal list to store memories
        private readonly List<Memory> _memories = new List<Memory>();
        public int Count => _memories.Count;

        // Method to add a memory
        public void AddMemory(Memory memory)
        {
            if (!_memories.Contains(memory))
            {
                _memories.Add(memory);
            }
        }

        // Method to remove a memory
        public void RemoveMemory(Memory memory)
        {
            if (_memories.Contains(memory))
            {
                _memories.Remove(memory);
            }
        }

        // Method to get all memories
        public List<Memory> GetAllMemories() => _memories.ToList();

        // Method to clear all memories
        public void Clear() => _memories.Clear();

        // Method to find memories by tag
        public List<Memory> FindMemoriesByTag(string tag) =>
            _memories.Where(m => m.Tags.Contains(tag)).ToList();

        // Method to find memories by date range
        public List<Memory> FindMemoriesByDateRange(DateTime start, DateTime end) =>
            _memories.Where(m => m.Date >= start && m.Date <= end).ToList();

        // Method to sort memories by date
        public List<Memory> GetMemoriesSortedByDate(bool ascending = true) =>
            ascending ? _memories.OrderBy(m => m.Date).ToList() : _memories.OrderByDescending(m => m.Date).ToList();

        // Method to sort memories by title
        public List<Memory> GetMemoriesSortedByTitle(bool ascending = true) =>
            ascending ? _memories.OrderBy(m => m.Title).ToList() : _memories.OrderByDescending(m => m.Title).ToList();

        // Method toString for easy display
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Memory Board:");
            foreach (var memory in _memories)
            {
                sb.AppendLine(memory.ToString());
            }
            return sb.ToString();
        }
    }
}