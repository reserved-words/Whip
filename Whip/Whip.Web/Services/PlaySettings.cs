using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Whip.Web.Interfaces;

namespace Whip.Web.Services
{
    public class PlaySettings : IPlaySettings
    {
        public bool Shuffle { get; set; }
    }
}