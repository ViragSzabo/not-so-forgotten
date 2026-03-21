using SQLite;

namespace Cemetery
{
    [Table("Memories")]
    public class MemoryDb
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string ImagePath { get; set; } = string.Empty;
        public string FavoriteSong { get; set; } = string.Empty;
    }
}
