using System;

namespace NotSoForgottenCemetery.Features
{
    public class Challenge
    {
        public Guid Id { get; private set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Difficulty { get; set; } // 1 = easy, 5 = hard
        public bool IsCompleted { get; private set; }

        // Constructor
        public Challenge(string title, string description, int difficulty)
        {
            Id = Guid.NewGuid();
            Title = title;
            Description = description;
            Difficulty = difficulty;
            IsCompleted = false;
        }

        // Method to mark the challenge as completed
        public void CompleteChallenge()
        {
            IsCompleted = true;
        }

        // Override ToString for better readability
        public override string ToString()
        {
            return $"{Title} (Difficulty: {Difficulty}) - {(IsCompleted ? "Completed" : "Not Completed")}";
        }
    }
}