using System;

namespace NotSoForgottenCemetery.Features
{
    public class Challenge(string title, string description, int difficulty)
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Title { get; set; } = title;
        public string Description { get; set; } = description;
        public int Difficulty { get; set; } = difficulty;
        public bool IsCompleted { get; private set; } = false;

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