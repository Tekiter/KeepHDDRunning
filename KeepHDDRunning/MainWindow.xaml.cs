using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.IO;

namespace KeepHDDRunning
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<Drive> drivelist;

        public ObservableCollection<Drive> DriveList { get => drivelist; set => drivelist = value; }

        public MainWindow()
        {
            InitializeComponent();
            
            DataContext = this;
            DriveList = new ObservableCollection<Drive>();
            UpdateDriveList();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            StartWorking();
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            UpdateDriveList();
        }

        void UpdateDriveList()
        {
            DriveList = new ObservableCollection<Drive>();

            DriveInfo[] info = DriveInfo.GetDrives();
            foreach (var i in info)
            {
                if (i.DriveType == DriveType.Fixed || i.DriveType == DriveType.Removable)
                {
                    Drive drive = new Drive(i.Name);
                    DriveList.Add(drive);
                }
            }
        }

        void StartWorking()
        {
            WorkingWindow window = new WorkingWindow(List_Drives.SelectedItem as Drive, int.Parse(TextBox_Interval.Text));
            window.Show();
            Close();
        }
    }
}
