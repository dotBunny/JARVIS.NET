using System;
namespace JARVIS.Core.Services.Spotify
{
    public class SpotifyTrack
    {
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
    }
}
