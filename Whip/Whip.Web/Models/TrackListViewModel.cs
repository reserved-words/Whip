using System;
using System.Collections.Generic;
using System.Linq;
using Whip.Common.Model;

namespace Whip.Web.Models
{
    public class TrackListViewModel
    {
        public TrackListViewModel(List<Track> tracks, int page, int tracksPerPage, Func<Track, TrackViewModel> getViewModel)
        {
            Page = page;

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
    }
}