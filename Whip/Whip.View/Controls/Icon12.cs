using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whip.Controls
{
    public class Icon12 : FontAwesome.WPF.ImageAwesome
    {
        public Icon12()
            : base()
        {
            VerticalAlignment = System.Windows.VerticalAlignment.Center;
            HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            Width = 12;
            Height = 12;
            SnapsToDevicePixels = true;
        }
    }
}
