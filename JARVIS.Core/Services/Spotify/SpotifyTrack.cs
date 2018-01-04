using System;
using System.Collections.Generic;

namespace JARVIS.Core.Services.Spotify
{
    public class SpotifyTrack
    {
        public class Image
        {
            public string URL;
            public int Height;
            public int Width;

            public Image(string href, int width, int height)
            {
                URL = href;
                Width = width;
                Height = height;
            }
        }

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
        public List<Image> Images;
        public string ImageURL;
        public string TrackURL;


        public byte[] ImageLargeData;


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

            if ( ImageLargeData != null && ImageLargeData.Length > 0) 
            {
                Database.Tables.KeyValueBytes.Set(SpotifyCurrentTrackImageKey, ImageLargeData); 
            }

        }
    }
}
