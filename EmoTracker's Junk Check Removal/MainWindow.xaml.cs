using System.IO;
using System.Windows;
using EmoTrackerJunkCheckRemoval.SpoilerLog;
using Microsoft.Win32;

namespace EmoTrackerJunkCheckRemoval
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly SpoilerLogFactory logFactory = new();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new();
            if (!dialog.ShowDialog().GetValueOrDefault()) return;
            txtSpoilerLogFilename.Text = dialog.FileName;
        }

        private void SaveTracker_Click(object sender, RoutedEventArgs e)
        {
            var spoilerLog = logFactory.Open(txtSpoilerLogFilename.Text);
            if (spoilerLog == null)
            {
                MessageBox.Show("Spoiler Log is invalid.");
                return;
            }
            SaveFileDialog dialog = new()
            {
                InitialDirectory = Path.Combine(App.GetEmoTrackerFolder(), "saves"),
                DefaultExt = "json",
                Filter = "EmoTracker Save File (*.json)|*.json"
            };
            if (!dialog.ShowDialog().GetValueOrDefault()) return;
            spoilerLog.SaveTracker(dialog.FileName);
        }
    }
}
