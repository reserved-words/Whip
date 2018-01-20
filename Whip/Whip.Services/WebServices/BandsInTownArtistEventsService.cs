using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Whip.Common.Exceptions;
using Whip.Common.Model;
using Whip.Services.Interfaces;
using Whip.Services.Interfaces.Singletons;

namespace Whip.Services
{
    public class BandsInTownArtistEventsService : IEventsService
    {
        private const string BandsInTownApiBaseUrl = "https://api.bandsintown.com/artists/{0}/events.json";
        private const string BandsInTownDateFormat = "MM/dd/yyyy HH:mm:ss";
        private const string ApiVersionParameterName = "api_version";
        private const string ApiVersionParameterValue = "2.0";
        private const string AppIdParameterName = "app_id";
        private const string JsonElementVenue = "venue";
        private const string JsonElementCountry = "country";
        private const string JsonElementCity = "city";
        private const string JsonElementVenueName = "name";
        private const string JsonElementDateTime = "datetime";
        private const string JsonElementArtists = "artists";
        private const string JsonElementArtistName = "name";
        private const string JsonElementErrors = "errors";

        private readonly IWebHelperService _webHelperService;
        private readonly Lazy<string> _apiId;

        public BandsInTownArtistEventsService(IWebHelperService webHelperService, IConfigSettings configSettings)
        {
            _apiId = new Lazy<string>(() => configSettings.BandsInTownApiKey);
            _webHelperService = webHelperService;
        }

        public async Task<bool> PopulateEventsAsync(Artist artist)
        {
            try
            {
                var baseUrl = string.Format(BandsInTownApiBaseUrl, _webHelperService.UrlEncode(artist.Name));

                var jsonString = await _webHelperService.HttpGetAsync(baseUrl, GetUrlParameters());

                HandleErrors(jsonString);

                artist.UpcomingEvents = GetEventsList(jsonString, artist.Name).OrderBy(ev => ev.Date).ToList();

                return true;
            }
            catch (ServiceException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Unexpected error while fetching artist event", ex);
            }
        }

        private Dictionary<string, string> GetUrlParameters()
        {
            return new Dictionary<string, string>
            {
                { ApiVersionParameterName, ApiVersionParameterValue },
                { AppIdParameterName, _apiId.Value }
            };
        }

        private static IEnumerable<ArtistEvent> GetEventsList(string jsonString, string artistName)
        {
            var list = new List<ArtistEvent>();

            try
            {
                var array = JArray.Parse(jsonString);

                foreach (var item in array)
                {
                    var dateTimeString = item[JsonElementDateTime].Value<string>().Trim();
                    var datetime = DateTime.ParseExact(dateTimeString, BandsInTownDateFormat, CultureInfo.InvariantCulture);

                    var venue = item[JsonElementVenue][JsonElementVenueName].Value<string>().Trim();
                    var city = item[JsonElementVenue][JsonElementCity].Value<string>().Trim();
                    var country = item[JsonElementVenue][JsonElementCountry].Value<string>().Trim();

                    var artists = item[JsonElementArtists];

                    var artistList = artists.Any()
                        ? string.Join(", ", artists.Select(a => a[JsonElementArtistName].Value<string>().Trim()))
                        : artistName;

                    list.Add(new ArtistEvent
                    {
                        Date = datetime,
                        Venue = venue,
                        City = city,
                        Country = country,
                        ArtistList = artistList
                    });
                }
            }
            catch (JsonReaderException)
            {
                // Can be ignored - means there were no results
            }

            return list;
        }

        private static void HandleErrors(string jsonString)
        {
            try
            {
                var error = JObject.Parse(jsonString);

                if (error?[JsonElementErrors] == null)
                    return;

                throw new ServiceException(
                    string.Join(Environment.NewLine, error[JsonElementErrors].Select(err => err.Value<string>())),
                    null);
            }
            catch (JsonReaderException)
            {
                // Can be ignored - means there were no errors
            }
        }
    }
}
