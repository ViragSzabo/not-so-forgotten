using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NotSoForgottenCemetery.Features
{
    public class Memory
    {
        // Properties
        public Guid Id { get; private set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public Song? AssociatedSong { get; set; }
        public List<string> Tags { get; private set; }

        // Constructor
        public Memory(string title, string description, DateTime date, Song? associatedSong = null)
        {
            Id = Guid.NewGuid();
            Title = title;
            Description = description;
            Date = date;
            AssociatedSong = associatedSong;
            Tags = new List<string>();
        }

        // Methods to manage tags
        public void AddTag(string tag)
        {
            if (!Tags.Contains(tag))
            {
                Tags.Add(tag);
            }
        }

        // Method to remove a tag
        public void RemoveTag(string tag)
        {
            if (Tags.Contains(tag))
            {
                Tags.Remove(tag);
            }
        }

        // Override ToString for easy display
        public override string ToString()
        {
            string songInfo = AssociatedSong != null ? $" 🎵 {AssociatedSong.Title} by {AssociatedSong.Artist}" : "";
            return $"{Title} ({Date.ToShortDateString()}) - {Description}{songInfo}";
        }

        // Override Equals and GetHashCode for proper comparison
        public override bool Equals(object obj) =>
            obj is Memory memory && Id == memory.Id;

        // Override GetHashCode
        public override int GetHashCode() => Id.GetHashCode();
    }
}