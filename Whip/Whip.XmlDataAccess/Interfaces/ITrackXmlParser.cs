using System.Xml.Linq;
using Whip.Common.Model;

namespace Whip.XmlDataAccess.Interfaces
{
    public interface ITrackXmlParser
    {
        Track GetTrack(XElement xml, Disc disc, Artist artist);
        Artist GetArtist(XElement artistXml);
        ArtistEvent GetEvent(XElement eventXml);
        Disc GetDisc(XElement discXml, Album album);
        Album GetAlbum(XElement albumXml, Artist albumArtist);
        XElement GetXElement(Track track);
        XElement GetXElement(Artist artist);
        XElement GetXElement(Album album);
        XElement GetXElement(Disc disc);
    }
}
