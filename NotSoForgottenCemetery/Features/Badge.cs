namespace NotSoForgottenCemetery.Features
{
    public class Badge
    {
        public Guid Id { get; private set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsUnlocked { get; private set; }

        // Constructor to initialize a badge with a title and description
        public Badge(string title, string description)
        {
            Id = Guid.NewGuid();
            Title = title;
            Description = description;
            IsUnlocked = false;
        }

        // Method to unlock the badge
        public void Unlock() => IsUnlocked = true;

        // Method to override the ToString method for better readability
        public override string ToString() => $"{Title} - {(IsUnlocked ? "🏆 Unlocked" : "🔒 Locked")}";
    }
}