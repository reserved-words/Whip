using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Whip.Common.Utilities;

namespace Whip.Controls
{
    public class TextBlockWithHyperlinks : TextBlock
    {
        private SolidColorBrush _hyperlinkForegroundBrush;

        public string Content
        {
            get { return (string)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register(nameof(Content), typeof(string), typeof(TextBlockWithHyperlinks), new PropertyMetadata(null, new PropertyChangedCallback(OnContentChanged)));

        private static void OnContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBlock = d as TextBlockWithHyperlinks;
            if (textBlock == null)
                return;

            textBlock.UpdateContent((string)e.NewValue);
        }

        public string HyperlinkColor
        {
            get { return (string)GetValue(HyperlinkColorProperty); }
            set { SetValue(HyperlinkColorProperty, value); }
        }

        public static readonly DependencyProperty HyperlinkColorProperty =
            DependencyProperty.Register(nameof(HyperlinkColor), typeof(string), typeof(TextBlockWithHyperlinks), new PropertyMetadata(null, new PropertyChangedCallback(OnHyperlinkColorChanged)));

        private static void OnHyperlinkColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBlock = d as TextBlockWithHyperlinks;
            if (textBlock == null)
                return;

            textBlock.UpdateHyperlinkColor((string)e.NewValue);
        }

        private void UpdateHyperlinkColor(string newValue)
        {
            _hyperlinkForegroundBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom(newValue));

            UpdateContent(Content);
        }

        private void UpdateContent(string newText)
        {
            Inlines.Clear();

            if (string.IsNullOrEmpty(newText))
                return;

            var patterns = HyperlinkRegexPattern.GetPatterns(HyperlinkRegexPatternType.HtmlAnchor);

            var matches = new List<Tuple<Match, HyperlinkRegexPattern>>();

            foreach (var pattern in patterns)
            {
                var regex = new Regex(pattern.Pattern);

                foreach (Match match in regex.Matches(newText))
                {
                    matches.Add(new Tuple<Match, HyperlinkRegexPattern>(match, pattern));
                }
            }

            matches = matches.OrderBy(m => m.Item1.Index).ToList();

            int last_pos = 0;
            foreach (var match in matches)
            {
                if (match.Item1.Index != last_pos)
                {
                    var raw_text = newText.Substring(last_pos, match.Item1.Index - last_pos);
                    Inlines.Add(new Run(raw_text));
                }

                Inlines.Add(MakeHyperlink(match.Item1.Groups[match.Item2.MatchUrlGroupIndex].ToString(), match.Item1.Groups[match.Item2.MatchTextGroupIndex].ToString()));

                last_pos = match.Item1.Index + match.Item1.Length;
            }

            if (last_pos < newText.Length)
            {
                Inlines.Add(new Run(newText.Substring(last_pos)));
            }
        }

        private Hyperlink MakeHyperlink(string url, string text = null)
        {
            var navigateUri = new Uri(url);

            var link = new Hyperlink(new Run(text ?? url))
            {
                NavigateUri = navigateUri,
                Foreground = _hyperlinkForegroundBrush,
                ToolTip = navigateUri
            };
            link.Click += OnUrlClick;

            return link;
        }

        private static void OnUrlClick(object sender, RoutedEventArgs e)
        {
            var link = (Hyperlink)sender;
            System.Diagnostics.Process.Start(link.NavigateUri.ToString());
        }
    }
}
