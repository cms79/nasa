using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Nasa.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Nasa
{
    public static class NasaApi
    {
        // Example full URL:
        // https://api.nasa.gov/mars-photos/api/v1/rovers/curiosity/photos?earth_date=2015-6-3&api_key=DEMO_KEY

        const string BASE_URL = "https://api.nasa.gov/mars-photos/api/v1/rovers/curiosity/";
        const string API_KEY = "wMzqDhU5vt772gRaVuXrRu7Z19uRKgCMCfMjUo4t";

        public static IList<Photo> GetMarsRoverPhotos(string earthDate)
        {
            // TODO: Should use asnyc here in case of slowness or other network issues

            var photos = new List<Photo>();

            var parameters = $"earth_date={earthDate}&api_key={API_KEY}";
            var httpRequest = (HttpWebRequest)WebRequest.Create($"{BASE_URL}/photos?{parameters}");
            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();

            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                var rootObject = JsonSerializer.Deserialize<Rootobject>(result);

                if (rootObject?.photos != null)
                {
                    photos.AddRange(rootObject.photos);
                }
            }

            return photos;
        }
    }
}
