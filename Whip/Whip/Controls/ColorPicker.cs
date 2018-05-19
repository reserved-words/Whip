using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using Whip.ViewModels.Utilities;
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
            return StandardColours.GetStandardColors()
                .Select(c => GetColorItem(c, string.Empty))
                .ToList();
        }

        private ColorItem GetColorItem(string hexCode, string name)
        {
            return new ColorItem((Color)ColorConverter.ConvertFromString(hexCode), name);
        }
    }
}
