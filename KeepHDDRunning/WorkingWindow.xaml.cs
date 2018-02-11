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
        public Folder SelectedFolder { get; set; }
        public int Interval { get; set; }
        public object FolderIcon { get => SelectedFolder.Icon; }
        public string FolderName { get => SelectedFolder.Name; }

        Thread workingThread;
        bool isRunning = true;

        public WorkingWindow()
        {
            InitializeComponent();
            DataContext = this;
            workingThread = new Thread(new ThreadStart(Work));
        }

        public WorkingWindow(Folder folder, int interval) : this()
        {
            SelectedFolder = folder;
            Interval = interval;
        }

        void Cancel()
        {
            //isRunning = false;
            
            workingThread.Abort();
            MainWindow window = new MainWindow(SelectedFolder);
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
                AccessDirs(SelectedFolder.Path);
            }
        }

        void AccessDirs(string path)
        {
            string[] dirs = Directory.GetDirectories(path);
            foreach (var i in dirs)
            {
                Thread.Sleep(Interval);
                Debug.WriteLine(i);
                AccessDirs(i);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            workingThread.Abort();
        }
    }
}
