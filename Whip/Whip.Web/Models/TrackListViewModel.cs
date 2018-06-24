using System;
using System.Collections.Generic;
using System.Linq;
using Whip.Common.Model;

namespace Whip.Web.Models
{
    public class TrackListViewModel
    {
        private readonly Func<int, string> _getPageUrl;

        public TrackListViewModel(List<Track> tracks, int page, int tracksPerPage, 
            Func<Track, TrackViewModel> getViewModel, Func<int, string> getPageUrl, string playUrl)
        {
            Page = page;
            PlayUrl = playUrl;
            _getPageUrl = getPageUrl;

            if (tracks == null)
            {
                Tracks = new List<TrackViewModel>();
                TotalPages = 1;
            }
            else
            {
                Tracks = tracks
                    .Skip((page - 1) * tracksPerPage)
                    .Take(tracksPerPage)
                    .Select(getViewModel)
                    .ToList();
                TotalPages = (int)Math.Ceiling((double)tracks.Count / tracksPerPage);
            }
        }

        public int Page { get; }
        public int TotalPages { get; }
        public List<TrackViewModel> Tracks { get; }
        public string PlayUrl { get; }

        public string FirstPageUrl => GetPageUrl(1);
        public string PreviousPageUrl => GetPageUrl(Page - 1);
        public string NextPageUrl => GetPageUrl(Page + 1);
        public string LastPageUrl => GetPageUrl(TotalPages);

        private string GetPageUrl(int page)
        {
            if (page > TotalPages || page < 1 || page == Page)
                return null;

            return _getPageUrl(page);
        }
    }
}