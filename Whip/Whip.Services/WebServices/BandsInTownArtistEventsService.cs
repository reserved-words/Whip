using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Whip.Common.Model;
using Whip.Services.Interfaces;

namespace Whip.Services
{
    public class BandsInTownArtistEventsService : IWebArtistEventsService
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

        public BandsInTownArtistEventsService(IWebHelperService webHelperService, IUserSettings userSettings)
        {
            _apiId = new Lazy<string>(() => userSettings.BandsInTownApiId);
            _webHelperService = webHelperService;
        }

        public async Task<List<ArtistEvent>> GetEventsAsync(Artist artist)
        {
            var baseUrl = string.Format(BandsInTownApiBaseUrl, _webHelperService.UrlEncode(artist.Name));

            var jsonString = await _webHelperService.HttpGetAsync(baseUrl, GetUrlParameters());

            HandleErrors(jsonString);

            return GetEventsList(jsonString, artist.Name).OrderBy(ev => ev.Date).ToList();
        }

        private Dictionary<string, string> GetUrlParameters()
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add(ApiVersionParameterName, ApiVersionParameterValue);
            parameters.Add(AppIdParameterName, _apiId.Value);
            return parameters;
        }

        private ICollection<ArtistEvent> GetEventsList(string jsonString, string artistName)
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

        private void HandleErrors(string jsonString)
        {
            try
            {
                var error = JObject.Parse(jsonString);

                if (error == null || error[JsonElementErrors] == null)
                    return;

                throw new Exception(string.Join(Environment.NewLine, error[JsonElementErrors].Select(err => err.Value<string>())));
            }
            catch (JsonReaderException)
            {
                // Can be ignored - means there were no errors
            }
        }
    }
}
