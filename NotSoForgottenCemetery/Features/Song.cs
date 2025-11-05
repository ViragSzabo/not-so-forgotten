using SociallyAnxiousHub.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotSoForgottenCemetery.Features
{
    class Song
    {
        private string _title;
        private string _artist;
        private string _album;
        private TimeSpan _duration;
        private string? _spotifyUrl;
        private string? CustomImgUrl;

        // Constructor
        public Song(string title, string artist, TimeSpan duration, string album, TimeSpan duration, string customImgUrl, string? spotifyUrl)
        {
            _title = title;
            _artist = artist;
            _duration = duration;
            _album = album;
            _spotifyUrl = spotifyUrl;
            CustomImgUrl = customImgUrl;
        }

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
            if (ReferenceEquals(left, null)) return ReferenceEquals(right, null);
            return left.Equals(right);
        }

        // Overloading != operator
        public static bool operator !=(Song left, Song right)
        {
            return !(left == right);
        }
    }
}