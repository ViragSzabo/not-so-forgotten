using SQLite;

namespace NotSoForgottenCemetery.Models
{
    [Table("Playlists")]
    public class PlaylistDb
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string SpotifyId { get; set; } = string.Empty;
    }
}
