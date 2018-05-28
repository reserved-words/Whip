using System;
using System.Xml.Linq;
using Whip.Services.Interfaces;

namespace Whip.Web.Services
{
    public class TrackXmlProvider : IXmlProvider
    {
        private const string Filename = "library.xml";

        private readonly ICloudService _cloudService;

        public TrackXmlProvider(ICloudService cloudService)
        {
            _cloudService = cloudService;
        }

        public XDocument Get()
        {
            var url = _cloudService.GetFileUrl(Filename);
            return XDocument.Load(url);
        }

        public void Save(XDocument doc)
        {
            throw new NotImplementedException();
        }
    }
}