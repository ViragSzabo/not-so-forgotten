using NotSoForgottenCemetery.Features;
using System.Text;

namespace SociallyAnxiousHub.Features
{
    class Playlist
    {
        private List<Song> _songs;
        private int _numberOfSongs;

        // Constructor
        public Playlist()
        {
            _songs = new List<Song>();
            _numberOfSongs = _songs.Count();
        }

        // Properties
        public void AddSong(Song song)
        {
            if (song != null && !_songs.Contains(song))
            {
                _songs.Add(song);
                _numberOfSongs++;
            }
        }

        // Methods
        public void RemoveSong(Song song)
        {
            if (song != null && _songs.Contains(song))
            {
                _songs.Remove(song);
                _numberOfSongs--;
            }
        }

        // Returns a copy of the list to prevent external modification
        public List<Song> GetSongs()
        {
            _numberOfSongs = _songs.Count;
            return new List<Song>(_songs);
        }

        // Returns the number of songs in the playlist
        public void ClearPlaylist()
        {
            _numberOfSongs = 0;
            _songs.Clear();
        }

        // Returns the number of songs in the playlist
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Playlist:");
            foreach (var song in _songs)
            {
                sb.AppendLine(song.ToString());
            }
            return sb.ToString();
        }

        // Equality based on the set of songs in the playlist
        public override bool Equals(object obj)
        {
            if (obj is Playlist playlist)
            {
                return _songs.SequenceEqual(playlist._songs);
            }
            return false;
        }

        // Helper method to compare two lists as sets
        public override int GetHashCode()
        {
            return _songs.Aggregate(0, (hash, song) => hash ^ song.GetHashCode());
        }

        // Shuffle the playlist using Fisher-Yates algorithm
        public void Shuffle()
        {
            Random rng = new Random(); // Static instance to avoid reseeding issues
            int n = _numberOfSongs; // Number of elements to shuffle
            while (n > 1) // While there are elements to shuffle
            {
                int k = rng.Next(n--); // Get a random index
                Song temp = _songs[n]; // Swap the last element with the random element
                _songs[n] = _songs[k]; // Swap
                _songs[k] = temp; // Swap
            }
        }

        // Sorting
        public void SortByTitle() // Sort by song title
        {
            _songs.Sort((x, y) => string.Compare(x.Title, y.Title, StringComparison.OrdinalIgnoreCase));
        }

        public void SortByArtist() // Sort by artist name
        {
            _songs.Sort((x, y) => string.Compare(x.Artist, y.Artist, StringComparison.OrdinalIgnoreCase));
        }

        public void SortByAlbum() // Sort by album name
        {
            _songs.Sort((x, y) => string.Compare(x.Album, y.Album, StringComparison.OrdinalIgnoreCase));
        }
    }
}