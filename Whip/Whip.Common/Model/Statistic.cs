using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whip.Common.Model
{
    public struct Statistic
    {
        public Statistic(string caption, object data, string url = null, string imageUrl = null)
        {
            Caption = caption;
            Data = data;
            Url = url;
            ImageUrl = imageUrl;
        }

        public string Caption { get; }
        public object Data { get; }
        public string Url { get; }
        public string ImageUrl { get; }
    }
}
