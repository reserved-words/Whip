using System.Xml.Linq;

namespace Whip.Services.Interfaces
{
    public interface IXmlProvider
    {
        XDocument Get();
        void Save(XDocument doc);
    }
}
