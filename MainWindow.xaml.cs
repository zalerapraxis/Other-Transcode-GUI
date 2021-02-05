using System.Linq;
using System.Text;
using System.IO;
using System.Windows;
using System.Windows.Media;

namespace Other_Transcode_GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private bool Last30s;
        private bool NoAudio;
        private bool OutputMP4;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_OnDrop(object sender, DragEventArgs e)
        {

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                HandleFileOpen(files);
            }
        }

        private void HandleFileOpen(string[] files)
        {
            foreach (var file in files)
            {
                // make and set output dir for file output
                var outputDir = $"{Path.GetDirectoryName(file)}\\output";
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // build launch command
                StringBuilder commandInput = new StringBuilder();
                commandInput.Append("/k other-transcode");

                // toggle arguments
                if (Last30s)
                {
                    var fileAnalysis = new MediaInfoDotNet.MediaFile(file);
                    var durationMilliseconds = fileAnalysis.Video.FirstOrDefault().Duration / 1000;

                    commandInput.Append($" --position {durationMilliseconds - 30}");
                }
                if (NoAudio)
                    commandInput.Append(" --main-audio 0");
                if (OutputMP4)
                    commandInput.Append(" --mp4");

                commandInput.Append($" \"{file}\" ");

                // build process
                var process = new System.Diagnostics.ProcessStartInfo();
                process.FileName = "cmd.exe";
                process.Arguments = commandInput.ToString();
                process.WorkingDirectory = outputDir;

                // run process
                System.Diagnostics.Process.Start(process);
            }
        }

        private void btnToggleLast30s_Click(object sender, RoutedEventArgs e)
        {
            if (!Last30s) // toggle on
            {
                btnToggleLast30s.Background = Brushes.Gold;
                Last30s = true;
            }
            else // toggle off
            {
                btnToggleLast30s.Background = new SolidColorBrush(Color.FromRgb(221, 221, 221));
                Last30s = false;
            }
        }

        private void btnToggleNoAudio_Click(object sender, RoutedEventArgs e)
        {
            if (!NoAudio) // toggle on
            {
                btnToggleNoAudio.Background = Brushes.Gold;
                NoAudio = true;
            }
            else // toggle off
            {
                btnToggleNoAudio.Background = new SolidColorBrush(Color.FromRgb(221, 221, 221));
                NoAudio = false;
            }

        }

        private void btnToggleOutputMP4_Click(object sender, RoutedEventArgs e)
        {
            if (!OutputMP4) // toggle on
            {
                btnToggleOutputMP4.Background = Brushes.Gold;
                OutputMP4 = true;
            }
            else // toggle off
            {
                btnToggleOutputMP4.Background = new SolidColorBrush(Color.FromRgb(221, 221, 221));
                OutputMP4 = false;
            }
        }
    }
}
