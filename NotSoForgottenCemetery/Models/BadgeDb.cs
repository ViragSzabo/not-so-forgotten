using SQLite;

namespace NotSoForgottenCemetery.Models
{
    [Table("Badges")]
    public class BadgeDb
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool Unlocked { get; set; }
    }
}
