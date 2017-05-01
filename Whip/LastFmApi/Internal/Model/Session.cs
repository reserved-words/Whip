using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastFmApi.Internal
{
    internal class Session
    {
        public Session(string key, string username)
        {
            Key = key;
            Username = username;
        }

        public string Key { get; private set; }
        public string Username { get; private set; }
    }
}
