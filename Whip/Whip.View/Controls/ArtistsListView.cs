using System;
using System.Windows.Controls;
using System.Windows.Input;
using Whip.Common.Model;

namespace Whip.Controls
{
    public class ArtistsListView : ListView
    {
        private KeyConverter keyConverter = new KeyConverter();

        public ArtistsListView()
        {
            KeyDown += ArtistsListView_KeyDown;
        }

        private void ArtistsListView_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;

            var keyChar = keyConverter.ConvertToString(e.Key).ToLower();

            if (!char.IsLetterOrDigit(Convert.ToChar(keyChar)))
                return;
            
            for (var i = SelectedIndex + 1; i < Items.Count; i++)
            {
                if (CheckArtist(i, keyChar))
                    return;
            }

            for (var i = 0; i < SelectedIndex; i++)
            {
                if (CheckArtist(i, keyChar))
                    return;
            }
        }

        private bool CheckArtist(int i, string character)
        {
            var artist = Items[i] as Artist;
            if (artist.Sort.ToLower().StartsWith(character))
            {
                SelectedItem = artist;
                ScrollIntoView(SelectedItem);
                return true;
            }

            return false;
        }
    }
}
