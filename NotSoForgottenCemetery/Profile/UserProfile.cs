using NotSoForgottenCemetery.Features.Memories;
using System;
using System.Collections.Generic;
using NotSoForgottenCemetery.Features;
using NotSoForgottenCemetery.Features.HabitItems;

namespace NotSoForgottenCemetery.Profile
{
	public class UserProfile(string userName)
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string UserName { get; set; } = userName;
        public MemoryBoard MemoryBoard { get; private set; } = new MemoryBoard();
        public MoodTracker MoodTracker { get; private set; } = new MoodTracker();
        public HabitTracker HabitTracker { get; private set; } = new HabitTracker();
        public List<Challenge> Challenges { get; private set; } = [];
        public List<Badge> Badges { get; private set; } = [];

        // Methods to add challenges and badges
        public void AddChallenge(Challenge challenge)
		{
			Challenges.Add(challenge);
		}

		// Method to add badges
		public void AddBadge(Badge badge)
		{
			Badges.Add(badge);
		}

		// Method to add Memories
		public void AddMemory(Memory memory)
		{
			MemoryBoard.AddMemory(memory);
		}

		// Method to log moods
		public void LogMood(MoodEntry moodEntry)
		{
            MoodTracker.LogMood(moodEntry);
		}

		// Method to add habits
		public void AddHabit(Habit habit)
		{
			HabitTracker.AddHabit(habit);
		}

		// Method to mark habit as completed
		public void CompleteHabit(Guid habitId)
		{
			HabitTracker.CompleteHabit(habitId);
		}

		// Method to get progress on habits
		public double GetHabitProgress(Guid habitId)
		{
			return HabitTracker.GetHabitProgress(habitId);
		}

		// Method to get mood history
		public List<MoodEntry> GetMoodHistory(DateTime from, DateTime to)
		{
			return MoodTracker.GetMoodHistory(from, to);
		}

		// Method to get all memories
		public List<Memory> GetAllMemories()
		{
			return MemoryBoard.GetAllMemories();
		}

		// Method to get all challenges
		public List<Challenge> GetAllChallenges()
		{
			return Challenges;
		}

		// Method to get all badges
		public List<Badge> GetAllBadges()
		{
			return Badges;
		}

		// Override ToString for easy display
		public override string ToString()
		{
			return $"UserProfile: {UserName}, Challenges: {Challenges.Count}, Badges: {Badges.Count}";
		}

		// Other methods to interact with the profile can be added here

		public void DisplayProfile()
		{
			Console.WriteLine(this.ToString());
			MemoryBoard.DisplayMemories();
            MoodTracker.DisplayMoods();
			HabitTracker.DisplayHabits();
		}

		public void ClearProfile()
		{
			MemoryBoard.ClearMemories();
			MoodTracker.ClearMoods();
			HabitTracker.ClearHabits();
			Challenges.Clear();
			Badges.Clear();
		}

		public void UpdateUserName(string newUserName)
		{
			if (!string.IsNullOrWhiteSpace(newUserName))
			{
				UserName = newUserName;
			}
		}
	}
}