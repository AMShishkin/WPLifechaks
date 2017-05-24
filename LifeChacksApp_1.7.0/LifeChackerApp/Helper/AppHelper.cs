using System.IO.IsolatedStorage;
using System.Windows.Media;

namespace LifeChacksApp
{
    static class AppHelper
    {
        public static int MAXSIZEBASEDB = 850;
        private static int _pageIndex;

        static AppHelper()
        {
            // Font
            BaseFontFamily = new FontFamily("/Fonts/BuxtonSketch.ttf#Buxton Sketch");
            StandartFontFamily = new FontFamily("Arial");
            Storage = IsolatedStorageSettings.ApplicationSettings;
        }

        public static IsolatedStorageSettings Storage { get; set; }

        // Flags
        public static bool IsRate { get; set; }
        public static bool AppBar { get; set; }
        public static bool AppEff { get; set; }
        public static bool AppFon { get; set; }
        public static bool IsTrial { get; set; }

        // Page index
        public static int PageIndex
        {
            set { if (value <= MAXSIZEBASEDB && value > -1) _pageIndex = value; }
            get { return _pageIndex; }
        }

        // Font
        public static FontFamily BaseFontFamily { get; private set; }
        public static FontFamily StandartFontFamily { get; private set; }
    }
}