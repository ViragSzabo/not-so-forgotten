using SQLite;

namespace NotSoForgottenCemetery.Models
{
    [Table("UserProfiles")]
    public class UserProfileDb
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string AvatarPath { get; set; } = string.Empty;
    }
}
