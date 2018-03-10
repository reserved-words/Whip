using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whip.Common.Model
{
    public struct Statistic
    {
        public Statistic(string caption, object data)
        {
            Caption = caption;
            Data = data;
        }

        public string Caption { get; }
        public object Data { get; }
    }
}
