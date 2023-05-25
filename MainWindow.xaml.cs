using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace player
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        Uri identificator, code;//идентификатор
        Taskwindow window;
        DispatcherTimer tim_vid_time;
        private void play_med_fail(object sender, ExceptionRoutedEventArgs e) { MessageBox.Show("ошибка во время открытия файла"); }

        private void playorpause_Click(object sender, RoutedEventArgs e)
        {
            if (playorpause.Content.ToString() == "play")
            {
                playorpause.Content = "pause";
                element.Play();
            }
            else
            {
                playorpause.Content = "play";
                element.Pause();
            }
        }

        private void resume_Click(object sender, RoutedEventArgs e) { element.Play(); }

        private void stop_Click(object sender, RoutedEventArgs e) { element.Stop(); }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) { if (element != null) element.Volume = slide.Value; }

        private void opening_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "video";
            dialog.DefaultExt = ".mp4";
            dialog.Filter = "video files (.mp4)|*.mp4";
            bool? result = dialog.ShowDialog();
            string filename;
            if (result == true)
            {
                filename = dialog.FileName;
                code = new Uri(filename);
                element.Source = code;
            }
        }

        private void click_of_new_file(object sender, RoutedEventArgs e)
        {
            window = new Taskwindow();
            window.Show();
        }

        private void collapse_Click(object sender, RoutedEventArgs e) { this.WindowState = WindowState.Minimized; }

        private void exit_Click(object sender, RoutedEventArgs e) { Close(); }

        private void start_Click(object sender, RoutedEventArgs e)
        {
            identificator = new Uri(box.Text);
            element.Source = identificator;
        }

        private void element_MediaOpened(object sender, RoutedEventArgs e)
        {
            tot_time = element.NaturalDuration.TimeSpan;
            tim_vid_time = new DispatcherTimer();
            tim_vid_time.Interval = TimeSpan.FromSeconds(1);
            tim_vid_time.Tick += new EventHandler(tim_tick);
            tim_vid_time.Start();
        }

        void tim_tick(object sender, EventArgs e)
        {
            if (element.NaturalDuration.TimeSpan.TotalSeconds > 0)
                if (tot_time.TotalSeconds > 0)
                {
                    int seconds = (int)element.Position.Seconds, minutes = (int)element.Position.TotalMinutes;//секунды в целочисленном виде
                    string mins = $"{minutes}:{seconds}";
                    if (minutes < 10)
                    {
                        if (seconds < 10)
                            mins = $"0{minutes}:0{seconds}";
                        else
                            mins = $"0{minutes}:{seconds}";
                    }
                    else
                    {
                        if (seconds < 10)
                            mins = $"{minutes}:0{seconds}";
                        else
                            mins = $"{minutes}:{seconds}";
                    }
                    block.Text = mins;
                }
        }

        private TimeSpan tot_time;
    }
}