using SQLite;

namespace NotSoForgottenCemetery.Models
{
    [Table("Habits")]
    public class HabitDb
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public DateTime Date { get; set; }
    }
}
