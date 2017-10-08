using System.IO;
using System.Xml.Linq;
using Whip.Services.Interfaces;

namespace Whip.Services
{
    public class XmlFileService : IXmlFileService
    {
        public XDocument Get(string path)
        {
            return File.Exists(path)
                ? XDocument.Load(path)
                : null;
        }

        public void Save(XDocument document, string path)
        {
            var file = new FileInfo(path);

            if (!Directory.Exists(file.DirectoryName))
            {
                Directory.CreateDirectory(file.DirectoryName);
            }

            document.Save(path);
        }
    }
}
