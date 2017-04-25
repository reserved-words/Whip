using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;

namespace Whip.Controls
{
    public class ColorPicker : Xceed.Wpf.Toolkit.ColorPicker
    {
        public ColorPicker()
        {
            StandardColors = new ObservableCollection<ColorItem>(GetStandardColors());
        }

        private List<ColorItem> GetStandardColors()
        {
            return new List<ColorItem>
            {
                GetColorItem("#696969", "Gray"),
                GetColorItem("#964256", "Red"),
                GetColorItem("#CE6C98", "Pink"),
                GetColorItem("#874691", "Purple"),
                GetColorItem("#4D64A3", "Blue"),
                GetColorItem("#48816B", "Green"),
                GetColorItem("#660000", "Dark Red"),
                GetColorItem("#3d0066", "Dark Purple"),
                GetColorItem("#00004d", "Dark Blue"),
                GetColorItem("#1a3300", "Dark Green"),
            };
        }

        private ColorItem GetColorItem(string hexCode, string name)
        {
            return new ColorItem((Color)ColorConverter.ConvertFromString(hexCode), name);
        }
    }
}
