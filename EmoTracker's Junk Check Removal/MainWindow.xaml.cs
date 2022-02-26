using System.IO;
using System.Windows;
using EmoTrackerJunkCheckRemoval.SpoilerLog;
using EmoTrackerJunkCheckRemoval.ViewModels;
using Microsoft.Win32;

namespace EmoTrackerJunkCheckRemoval
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly SpoilerLogFactory logFactory = new();
        private ISpoilerLog openedSpoilerLog;

        internal SpoilerLogViewModel SpoilerLogViewModel { get; } = new();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = SpoilerLogViewModel;
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new();
            if (!dialog.ShowDialog().GetValueOrDefault()) return;
            txtSpoilerLogFilename.Text = dialog.FileName;

            openedSpoilerLog = logFactory.Open(txtSpoilerLogFilename.Text);
            if (openedSpoilerLog == null)
                MessageBox.Show("Spoiler Log is invalid.");
            SpoilerLogViewModel.SetSpoilerLog(openedSpoilerLog);
        }

        private void SaveTracker_Click(object sender, RoutedEventArgs e)
        {
            if (openedSpoilerLog == null) return;

            SaveFileDialog dialog = new()
            {
                InitialDirectory = Path.Combine(App.GetEmoTrackerFolder(), "saves"),
                DefaultExt = "json",
                Filter = "EmoTracker Save File (*.json)|*.json"
            };
            if (!dialog.ShowDialog().GetValueOrDefault()) return;
            openedSpoilerLog.SaveTracker(dialog.FileName);
        }
    }
}
