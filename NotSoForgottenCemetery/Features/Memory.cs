using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NotSoForgottenCemetery.Features
{
    public class Memory(string title, string description, DateTime date, Song? associatedSong = null)
    {
        // Properties
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Title { get; set; } = title;
        public string Artist { get; set; }
        public string Description { get; set; } = description;
        public DateTime Date { get; set; } = date;
        public Song? AssociatedSong { get; set; } = associatedSong;
        public List<string> Tags { get; private set; } = [];

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
            Tags.Remove(tag);
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