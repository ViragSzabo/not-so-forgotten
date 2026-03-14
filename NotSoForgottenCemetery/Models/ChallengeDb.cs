using SQLite;

namespace NotSoForgottenCemetery.Models
{
    [Table("Challenges")]
    public class ChallengeDb
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
    }
}
