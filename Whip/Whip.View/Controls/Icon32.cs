using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whip.Controls
{
    public class Icon32 : FontAwesome.WPF.ImageAwesome
    {
        public Icon32()
            :base()
        {
            VerticalAlignment = System.Windows.VerticalAlignment.Center;
            HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            Width = 32;
            Height = 32;
            SnapsToDevicePixels = true;
        }
    }
}
