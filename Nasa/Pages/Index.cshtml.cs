using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Nasa.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Nasa.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public List<Photo> Photos { get; set; } = new List<Photo>();

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            Photos.Clear();

            foreach (var dateTime in ConvertDates(GetDateStringsFromFile()))
            {
                var photos = NasaApi.GetMarsRoverPhotos(dateTime.ToString("yyyy-M-d"));
                Photos.AddRange(photos);
            }

            // Hard coded example
            //Photos = NasaApi.GetMarsRoverPhotos("2015-6-3");
        }

        private IList<string> GetDateStringsFromFile()
        {
            var dateStrings = new List<string>();

            try
            {
                using var sr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "/dates.txt");
                dateStrings.AddRange(sr.ReadToEnd().Split("\r\n"));
            }
            catch (Exception error)
            {
                _logger.LogError(error.Message);
            }

            return dateStrings;
        }

        private IList<DateTime> ConvertDates(IList<string> dateStrings)
        {
            var dateList = new List<DateTime>();

            foreach (string dateString in dateStrings)
            {
                if (DateTime.TryParse(dateString, out DateTime dateTime))
                {
                    dateList.Add(dateTime);
                }
                else
                {
                    _logger.LogError($"Could not convert to DateTime object: {dateString}");
                }
            }

            return dateList;
        }
    }
}
