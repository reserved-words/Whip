using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whip.Controls
{
    public class Icon16 : FontAwesome.WPF.ImageAwesome
    {
        public Icon16()
            : base()
        {
            VerticalAlignment = System.Windows.VerticalAlignment.Center;
            HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            Width = 16;
            Height = 16;
            SnapsToDevicePixels = true;
        }
    }
}
