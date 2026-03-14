using SQLite;

namespace NotSoForgottenCemetery.Models
{
    [Table("Whispers")]
    public class WhisperDb
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
