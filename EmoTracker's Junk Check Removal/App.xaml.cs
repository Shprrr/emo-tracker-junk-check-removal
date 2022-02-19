using System;
using System.IO;
using System.Windows;

namespace EmoTrackerJunkCheckRemoval
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string GetEmoTrackerFolder()
        {
            string userDirectoryLocalPath = "EmoTracker";

            string docsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), userDirectoryLocalPath);
            if (Directory.Exists(docsPath))
                return docsPath;

            string appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), userDirectoryLocalPath);
            return Directory.Exists(appDataPath) ? appDataPath : "";
        }
    }
}
