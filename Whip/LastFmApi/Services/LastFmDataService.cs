using LastFmApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastFmApi
{
    public class LastFmDataService : ILastFmDataService
    {
        private readonly ApiClient _client;

        public LastFmDataService(ApiClient client)
        {
            _client = client;
        }
    }
}
