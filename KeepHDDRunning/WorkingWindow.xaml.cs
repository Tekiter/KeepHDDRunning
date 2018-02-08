using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KeepHDDRunning
{
    /// <summary>
    /// WorkingWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class WorkingWindow : Window
    {
        public Drive SelectedDrive { get; set; }
        public int Interval { get; set; }
        public object DriveIcon { get => SelectedDrive.Icon; }
        public string DriveName { get => SelectedDrive.Name; }

        Thread workingThread;
        bool isRunning = true;

        public WorkingWindow()
        {
            InitializeComponent();
            DataContext = this;
            workingThread = new Thread(new ThreadStart(Work));
        }

        public WorkingWindow(Drive drive, int interval) : this()
        {
            SelectedDrive = drive;
            Interval = interval;
        }

        void Cancel()
        {
            //isRunning = false;
            
            workingThread.Abort();
            MainWindow window = new MainWindow();
            window.Show();
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Cancel();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            workingThread.Start();
        }

        void Work()
        {
            while (isRunning)
            {
                string[] path = Directory.GetDirectories(SelectedDrive.Name);
                Debug.WriteLine("Flash!");
                Thread.Sleep(Interval); 
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            workingThread.Abort();
        }
    }
}
