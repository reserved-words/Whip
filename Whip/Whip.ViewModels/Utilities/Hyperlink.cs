namespace Whip.ViewModels.Utilities
{
    public static class Hyperlink
    {
        public static void Go(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                System.Diagnostics.Process.Start(url);
            }
        }
    }
}
