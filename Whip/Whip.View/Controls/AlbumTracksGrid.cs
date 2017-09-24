using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using GalaSoft.MvvmLight.Command;
using Whip.Common.Model;
using Whip.View.Views;

namespace Whip.Controls
{
    public class AlbumTracksGrid : Grid
    {
        private const int WidthTrackNoPixels = 40;
        private const int WidthTrackDurationPixels = 40;
        private const int MinimumAlbumRowSpan = 12;
        private const string TrackNoFormat = "#00";
        private const string TrackDurationFormat = @"mm\:ss";

        private readonly Brush _highlightForegroundBrush = Brushes.White;
        private readonly Brush _defaultForegroundBrush = Brushes.Black;
        private readonly Brush _defaultBackgroundBrush = Brushes.White;
        private readonly Brush _separatorLineBrush = Brushes.Silver;
        
        private int _populatingRow;
        private int _selectedTrackRow;
        private bool _justClickedTrack;
        private Brush _highlightBackgroundBrush;

        public AlbumTracksGrid()
        {
            ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(WidthTrackNoPixels, GridUnitType.Pixel) });
            ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(WidthTrackDurationPixels, GridUnitType.Pixel) });

            ContextMenuOpening += AlbumTracksGrid_ContextMenuOpening;

            SetHighlightBrush();
        }

        public Artist Artist
        {
            get { return (Artist)GetValue(ArtistProperty); }
            set { SetValue(ArtistProperty, value); }
        }

        public static readonly DependencyProperty ArtistProperty =
            DependencyProperty.Register(nameof(Artist), typeof(Artist), typeof(AlbumTracksGrid), new PropertyMetadata(null, OnArtistChanged));
        
        public Track SelectedTrack
        {
            get { return (Track)GetValue(SelectedTrackProperty); }
            set { SetValue(SelectedTrackProperty, value); }
        }

        public static readonly DependencyProperty SelectedTrackProperty =
            DependencyProperty.Register(nameof(SelectedTrack), typeof(Track), typeof(AlbumTracksGrid), new PropertyMetadata(null));
        
        public Album SelectedAlbum
        {
            get { return (Album)GetValue(SelectedAlbumProperty); }
            set { SetValue(SelectedAlbumProperty, value); }
        }

        public static readonly DependencyProperty SelectedAlbumProperty =
            DependencyProperty.Register(nameof(SelectedAlbum), typeof(Album), typeof(AlbumTracksGrid), new PropertyMetadata(null));

        public RelayCommand AlbumDoubleClickCommand
        {
            get { return (RelayCommand)GetValue(AlbumDoubleClickCommandProperty); }
            set { SetValue(AlbumDoubleClickCommandProperty, value); }
        }

        public static readonly DependencyProperty AlbumDoubleClickCommandProperty =
            DependencyProperty.Register(nameof(AlbumDoubleClickCommand), typeof(RelayCommand), typeof(AlbumTracksGrid), new PropertyMetadata(null));

        public RelayCommand TrackDoubleClickCommand
        {
            get { return (RelayCommand)GetValue(TrackDoubleClickCommandProperty); }
            set { SetValue(TrackDoubleClickCommandProperty, value); }
        }

        public static readonly DependencyProperty TrackDoubleClickCommandProperty =
            DependencyProperty.Register(nameof(TrackDoubleClickCommand), typeof(RelayCommand), typeof(AlbumTracksGrid), new PropertyMetadata(null));

        public string HighlightColour
        {
            get { return (string)GetValue(HighlightColourProperty); }
            set { SetValue(HighlightColourProperty, value); }
        }

        public static readonly DependencyProperty HighlightColourProperty =
            DependencyProperty.Register(nameof(HighlightColour), typeof(string), typeof(AlbumTracksGrid), new PropertyMetadata(null, OnHighlightColourChanged));

        private static void OnHighlightColourChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var albumTracksGrid = d as AlbumTracksGrid;
            albumTracksGrid?.SetHighlightBrush();
            albumTracksGrid?.HighlightSelectedRow();
        }

        private static void OnArtistChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var albumTracksGrid = d as AlbumTracksGrid;
            albumTracksGrid?.UpdateArtist();
        }

        private void AlbumTracksGrid_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            e.Handled = !_justClickedTrack;
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            _justClickedTrack = false;

            SelectedAlbum = GetSelectedAlbum(e);

            if (SelectedAlbum != null)
            {
                ProcessDoubleClick(e, AlbumDoubleClickCommand);
            }
            else
            {
                var selectedLabel = GetSelectedLabel(e);
                var selectedTrack = selectedLabel?.Tag as Track;

                if (selectedTrack != null)
                {
                    _justClickedTrack = true;
                    _selectedTrackRow = GetRow(selectedLabel);
                    HighlightSelectedRow();
                    SelectedTrack = selectedTrack;
                    ProcessDoubleClick(e, TrackDoubleClickCommand);
                }
            }

            base.OnMouseDown(e);
        }

        private static void ProcessDoubleClick(MouseButtonEventArgs e, ICommand command)
        {
            if (e.ClickCount != 2)
                return;

            if (command != null && command.CanExecute(null))
            {
                command.Execute(null);
            }
        }

        private void HighlightSelectedRow()
        {
            foreach (var label in Children.OfType<Label>())
            {
                var highlight = GetRow(label) == _selectedTrackRow;
                label.Foreground = highlight ? _highlightForegroundBrush : _defaultForegroundBrush;
                label.Background = highlight ? _highlightBackgroundBrush : _defaultBackgroundBrush;
            }
        }

        private void UpdateArtist()
        {
            ResetGrid();

            if (Artist == null)
                return;

            foreach (var album in Artist.Albums)
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

        private void ResetGrid()
        {
            Children.Clear();
            RowDefinitions.Clear();
            _populatingRow = 0;
            _selectedTrackRow = -1;
            _justClickedTrack = false;
        }

        private void AddAlbumSummary(Album album, int rowSpan)
        {
            UIElement albumUserControl = new LibraryAlbumView { DataContext = album };
            AddControl(albumUserControl, 0, _populatingRow, 1, rowSpan);
        }

        private void AddBlankLabel()
        {
            AddControl(CreateLabel(""), 1, _populatingRow++, 4);
        }

        private void AddDividingLine()
        {
            UIElement horizontalLine = new Border
            {
                BorderBrush = _separatorLineBrush,
                BorderThickness = new Thickness(0,0,0,1),
                Margin = new Thickness(0,2,0,5)
            };
            AddControl(horizontalLine, 0, _populatingRow++, 5);
        }

        private void AddDiscData(Disc disc)
        {
            AddControl(CreateLabel($"Disc {disc.DiscNo}", null, true), 1, _populatingRow++, 4);
        }

        private void AddTrackData(Track track)
        {
            AddControl(CreateLabel(track.TrackNo.ToString(TrackNoFormat), track), 1, _populatingRow);
            AddControl(CreateLabel(track.Title, track), 2, _populatingRow);
            AddControl(CreateLabel(track.Artist.Name, track), 3, _populatingRow);
            AddControl(CreateLabel(track.Duration.ToString(TrackDurationFormat), track), 4, _populatingRow);
            _populatingRow++;
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

        private void SetHighlightBrush()
        {
            if (HighlightColour == null)
                return;

            _highlightBackgroundBrush = new BrushConverter().ConvertFrom(HighlightColour) as SolidColorBrush;
        }

        private static Label GetSelectedLabel(MouseButtonEventArgs e)
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
            
            return contentPresenter?.TemplatedParent as Label;
        }

        private static Album GetSelectedAlbum(MouseButtonEventArgs e)
        {
            var control = e.OriginalSource as FrameworkElement;
            var stackPanel = control?.Parent as FrameworkElement;
            var libraryAlbumView = stackPanel?.Parent as LibraryAlbumView;
            return libraryAlbumView?.DataContext as Album;
        }
    }
}
