using System;
namespace JARVIS.Core.Services.Spotify
{
    public class SpotifyTrack
    {
        public const string SpotifyCurrentTrackKey = "Spotify.CurrentTrack";
        public const string SpotifyCurrentArtistKey = "Spotify.CurrentArtist";
        public const string SpotifyCurrentAlbumKey = "Spotify.CurrentAlbum";
        public const string SpotifyCurrentTrackURLKey = "Spotify.CurrentTrackURL";
        public const string SpotifyCurrentImageURLKey = "Spotify.CurrentImageURL";
        public const string SpotifyCurrentTrackImageKey = "Spotify.CurrentTrackImage";

        public string ID = "0";
        public string Track = "At This Time";
        public string Album;
        public string Artist = "Spotify Unavailable";
        public string ImageURL;
        public string TrackURL;

        public byte[] ImageData;

        public override string ToString()
        {
            return "[" + ID + "] " + Artist + " - " + Track;
        }
        public string ToInfoString()
        {
            return Artist + " - " + Track;
        }

        public void SaveToDatabase()
        {
            Database.Tables.KeyValueString.Set(SpotifyCurrentArtistKey, Artist); 
            Database.Tables.KeyValueString.Set(SpotifyCurrentTrackKey, Track);
            Database.Tables.KeyValueString.Set(SpotifyCurrentAlbumKey, Album); 
            Database.Tables.KeyValueString.Set(SpotifyCurrentTrackURLKey, TrackURL); 
            Database.Tables.KeyValueString.Set(SpotifyCurrentImageURLKey, ImageURL); 

            if ( ImageData != null && ImageData.Length > 0) 
            {
                Database.Tables.KeyValueBytes.Set(SpotifyCurrentTrackImageKey, ImageData); 
            }

        }
    }
}
