using System;
using System.Collections.Generic;

namespace JARVIS.Core.Services.Spotify.JSON
{
    public class CurrentlyPlayingResponse
    {
        public static string Endpoint = "/v1/me/player/currently-playing/";

        public Context context { get; set; }
        public long timestamp { get; set; }
        public int progress_ms { get; set; }
        public bool is_playing { get; set; }
        public Item item { get; set; }


        public string TrackID { 
            get
            {
                return item.id;   
            }
        }


        public SpotifyTrack GetTrack()
        {
            SpotifyTrack newTrack = new SpotifyTrack();
           
            // Unique ID
            newTrack.ID = item.id;

            // Process Artist Information
            string artist = string.Empty;
            foreach(Artist a in item.artists)
            {
                artist += a.name + ", ";
            }

            if (artist.EndsWith(", ", StringComparison.Ordinal))
            {
                artist = artist.Substring(0, artist.LastIndexOf(", ", StringComparison.Ordinal));   
            }
            newTrack.Artist = artist;

            // Process Track
            newTrack.Track = item.name;

            newTrack.Album = item.album.name;

            // URL
            if (item.external_urls.ContainsKey("spotify"))
            {
                newTrack.TrackURL = item.external_urls["spotify"];
            }

            // Image
            if (item.album.images.Count > 0)
            {
                newTrack.ImageURL = item.album.images[0].url;
            }

            return newTrack;
        }
    }


    public class Context
    {
        public Dictionary<string, string> external_urls { get; set; }
        public string href { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }

    public class Image
    {
        public int height { get; set; }
        public string url { get; set; }
        public int width { get; set; }
    }

    public class Album
    {
        public string album_type { get; set; }
        public Dictionary<string, string> external_urls { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public List<Image> images { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }

    public class Artist
    {
        public Dictionary<string, string> external_urls { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }

    public class Item
    {
        public Album album { get; set; }
        public List<Artist> artists { get; set; }
        public List<string> available_markets { get; set; }
        public int disc_number { get; set; }
        public int duration_ms { get; set; }
        public bool @explicit { get; set; }
        public Dictionary<string, string> external_ids { get; set; }
        public Dictionary<string, string> external_urls { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public int popularity { get; set; }
        public string preview_url { get; set; }
        public int track_number { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }
}