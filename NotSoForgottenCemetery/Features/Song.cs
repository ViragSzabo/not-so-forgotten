namespace NotSoForgottenCemetery.Features
{
    public class Song(string title, string artist, TimeSpan duration, string album, string customImgUrl, string? spotifyUrl)
    {
        private string _title = title;
        private string _artist = artist;
        private string _album = album;
        private TimeSpan _duration = duration;
        private string? _spotifyUrl = spotifyUrl;
        private readonly string? CustomImgUrl = customImgUrl;

        // Properties
        // Getters and Setters
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public string Artist
        {
            get { return _artist; }
            set { _artist = value; }
        }

        public string Album
        {
            get { return _album; }
            set { _album = value; }
        }

        public string SpotifyUrl
        {
            get { return _spotifyUrl; }
            set { _spotifyUrl = value; }
        }

        public TimeSpan Duration
        {
            get { return _duration; }
            set { _duration = value; }
        }

        public string GetCustomImagePath()
        {
            return CustomImgUrl;
        }

        // String representation of the song
        public override string ToString()
        {
            return $"{Title} by {Artist} from the album {Album}";
        }

        // Equality based on title, artist, and album (case insensitive)
        public override bool Equals(object obj)
        {
            if (obj is Song song)
            {
                return Title.Equals(song.Title, StringComparison.OrdinalIgnoreCase) &&
                       Artist.Equals(song.Artist, StringComparison.OrdinalIgnoreCase) &&
                       Album.Equals(song.Album, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        // Hash code based on title, artist, and album (case insensitive)
        public override int GetHashCode()
        {
            return HashCode.Combine(Title.ToLowerInvariant(), Artist.ToLowerInvariant(), Album.ToLowerInvariant());
        }

        // Overloading == and != operators
        public static bool operator ==(Song left, Song right)
        {
            if (left is null) return right is null;
            return left.Equals(right);
        }

        // Overloading != operator
        public static bool operator !=(Song left, Song right)
        {
            return !(left == right);
        }
    }
}