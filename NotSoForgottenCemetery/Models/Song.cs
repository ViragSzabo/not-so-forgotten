namespace NotSoForgottenCemetery.Models
{
    public class Song(string title, string artist, TimeSpan duration, string album, string customImgUrl, string? spotifyUrl)
    {
        public string Title { get; set; } = title;
        public string Artist { get; set; } = artist;
        public string Album { get; set; } = album;
        public TimeSpan Duration { get; set; } = duration;
        public string? SpotifyUrl { get; set; } = spotifyUrl;
        public string? CustomImgUrl { get; } = customImgUrl;

        public string? GetCustomImagePath() => CustomImgUrl;

        public override string ToString() => $"{Title} by {Artist} from the album {Album}";

        public override bool Equals(object? obj)
        {
            if (obj is Song song)
            {
                return Title.Equals(song.Title, StringComparison.OrdinalIgnoreCase) &&
                       Artist.Equals(song.Artist, StringComparison.OrdinalIgnoreCase) &&
                       Album.Equals(song.Album, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        public override int GetHashCode() =>
            HashCode.Combine(Title.ToLowerInvariant(), Artist.ToLowerInvariant(), Album.ToLowerInvariant());

        public static bool operator ==(Song? left, Song? right)
        {
            if (left is null) return right is null;
            return left.Equals(right);
        }

        public static bool operator !=(Song? left, Song? right) => !(left == right);
    }
}
