using SQLite;

namespace Cemetery
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
