using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Whip.Common.Model;
using Whip.View.Views;

namespace Whip.Controls
{
    public class AlbumTracksGrid : Grid
    {
        /*
         *  TO DO:
         *  Double-click album to play album in order
         *  Double-click track to play all artist tracks in order (starting with that track)
         *  Right-click to get standard track context menu (Edit Track, Add to Playlist)
         *  If possible highlight colour should update when updated in settings
         */
        
        private const int WidthTrackNoPixels = 40;
        private const int WidthTrackDurationPixels = 40;
        private const int MinimumAlbumRowSpan = 12;

        private int _currentRow;

        public AlbumTracksGrid()
        {
            ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(WidthTrackNoPixels, GridUnitType.Pixel) });
            ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(20, GridUnitType.Star) });
            ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(20, GridUnitType.Star) });
            ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(WidthTrackDurationPixels, GridUnitType.Pixel) });
        }

        public Artist Artist
        {
            get { return (Artist)GetValue(ArtistProperty); }
            set { SetValue(ArtistProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Artist.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ArtistProperty =
            DependencyProperty.Register("Artist", typeof(Artist), typeof(AlbumTracksGrid), new PropertyMetadata(null, OnArtistChanged));
        
        public Track SelectedTrack
        {
            get { return (Track)GetValue(SelectedTrackProperty); }
            set { SetValue(SelectedTrackProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedTrack.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedTrackProperty =
            DependencyProperty.Register("SelectedTrack", typeof(Track), typeof(AlbumTracksGrid), new PropertyMetadata(null));
        
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            var contentPresenter = GetClickedContentPresenter(e);
            var selectedLabel = contentPresenter?.TemplatedParent as Label;
            var selectedTrack = selectedLabel?.Tag as Track;

            if (selectedTrack != null)
            {
                SelectedTrack = selectedTrack;

                var selectedRow = GetRow(selectedLabel);

                foreach (var label in Children.OfType<Label>())
                {
                    var row = GetRow(label);
                    label.Foreground = row == selectedRow ? Brushes.White : Brushes.Black;
                    label.Background = row == selectedRow ? GetMainColorBrush() : Brushes.White;
                }
            }
            
            base.OnMouseUp(e);
        }

        private static void OnArtistChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var albumTracksGrid = d as AlbumTracksGrid;
            
            albumTracksGrid?.Update((Artist)e.NewValue);
        }

        private void Update(Artist artist)
        {
            Children.Clear();
            RowDefinitions.Clear();
            _currentRow = 0;

            foreach (var album in artist.Albums)
            {
                var totalRowsNeeded = TotalRowsNeeded(album);
                var totalRowsToDisplay = Math.Max(totalRowsNeeded, MinimumAlbumRowSpan);
                
                for (var i = 0; i < totalRowsToDisplay + 1; i++)
                {
                    AddRow();
                }

                AddAlbumSummary(album, totalRowsToDisplay);
                
                foreach (var disc in album.Discs)
                {
                    if (DisplayDiscHeadings(album))
                    {
                        AddDiscData(disc);
                    }

                    foreach (var track in disc.Tracks)
                    {
                        AddTrackData(track);
                    }
                }

                for (var i = 0; i < totalRowsToDisplay - totalRowsNeeded; i++)
                {
                    AddBlankLabel();
                }

                AddDividingLine();
            }
        }

        private void AddAlbumSummary(Album album, int rowSpan)
        {
            UIElement albumUserControl = new LibraryAlbumView { DataContext = album };
            AddControl(albumUserControl, 0, _currentRow, 1, rowSpan);
        }

        private void AddBlankLabel()
        {
            AddControl(CreateLabel(""), 1, _currentRow++, 4);
        }

        private void AddDividingLine()
        {
            UIElement horizontalLine = new Border
            {
                BorderBrush = Brushes.Silver,
                BorderThickness = new Thickness(0,0,0,1),
                Margin = new Thickness(0,2,0,5)
            };
            AddControl(horizontalLine, 0, _currentRow++, 5);
        }

        private void AddDiscData(Disc disc)
        {
            AddControl(CreateLabel($"Disc {disc.DiscNo}", true), 1, _currentRow++, 4);
        }

        private void AddTrackData(Track track)
        {
            AddControl(CreateLabel(track.TrackNo.ToString("#00"), track), 1, _currentRow);
            AddControl(CreateLabel(track.Title, track), 2, _currentRow);
            AddControl(CreateLabel(track.Artist.Name, track), 3, _currentRow);
            AddControl(CreateLabel(track.Duration.ToString(@"mm\:ss"), track), 4, _currentRow);
            _currentRow++;
        }

        private static int TotalRowsNeeded(Album album)
        {
            var trackCount = album.Discs.SelectMany(d => d.Tracks).Count();
            return trackCount + (DisplayDiscHeadings(album) ? album.Discs.Count : 0);
        }

        private static UIElement CreateLabel(string content, object tag = null, bool bold = false)
        {
            return new Label
            {
                Content = content,
                FontWeight = bold ? FontWeights.Bold : FontWeights.Normal,
                Padding = new Thickness(1),
                HorizontalContentAlignment = HorizontalAlignment.Stretch,
                Tag = tag
            };
        }

        private static bool DisplayDiscHeadings(Album album)
        {
            return album.Discs.Count > 1;
        }

        private void AddRow()
        {
            RowDefinitions.Add(new RowDefinition { Height = new GridLength(20, GridUnitType.Pixel) });
        }

        private void AddControl(UIElement element, int column, int row, int columnSpan = 1, int rowSpan = 1)
        {
            SetColumn(element, column);
            SetRow(element, row);
            SetRowSpan(element, rowSpan);
            SetColumnSpan(element, columnSpan);
            Children.Add(element);
        }

        private Brush GetMainColorBrush()
        {
            return new BrushConverter().ConvertFrom(View.Properties.Settings.Default.MainColourRgb) as SolidColorBrush;
        }

        private ContentPresenter GetClickedContentPresenter(MouseButtonEventArgs e)
        {
            ContentPresenter contentPresenter;

            var textBlock = e.OriginalSource as TextBlock;
            if (textBlock != null)
            {
                contentPresenter = textBlock.TemplatedParent as ContentPresenter;
            }
            else
            {
                var border = e.OriginalSource as Border;
                contentPresenter = border?.Child as ContentPresenter;
            }

            return contentPresenter;
        }
    }
}
