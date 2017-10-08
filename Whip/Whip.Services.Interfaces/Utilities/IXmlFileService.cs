using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Whip.Services.Interfaces
{
    public interface IXmlFileService
    {
        XDocument Get(string path);
        void Save(XDocument document, string path);
    }
}
