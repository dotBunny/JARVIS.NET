using System;
namespace JARVIS.Core.Services.Spotify
{
    public class SpotifyTrack
    {
        public string ID;
        public string Track;
        public string Album;
        public string Artist;
        public string ImageURL;
        public string TrackURL;

        public override string ToString()
        {
            return "[" + ID + "] " + Artist + " - " + Track;
        }
    }
}
