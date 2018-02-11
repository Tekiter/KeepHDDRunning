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
        

       
        Folder SelectedFolder;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            SelectedFolder = new Folder( DriveInfo.GetDrives()[0].Name);
        }

        public MainWindow(Folder StartingFolder) : this()
        {
            SelectedFolder = StartingFolder;

        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedFolder.Exists && int.TryParse(TextBox_Interval.Text,out int result))
            {
                WorkingWindow ww = new WorkingWindow(SelectedFolder, result);
                ww.Show();
                Close();
            }
        }

        private void Change_Click(object sender, RoutedEventArgs e)
        {
            SelectFolderDialog dia = new SelectFolderDialog();
            if (dia.ShowDialog() == true)
            {
                SelectedFolder = new Folder(dia.SelectedFolder);
                UpdateFolderInfo();
            }
        }

        void UpdateFolderInfo()
        {
            Image_Icon.Source = (ImageSource)SelectedFolder.Icon;
            TextBlock_Folder.Text = SelectedFolder.Name;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateFolderInfo();
        }
    }
}
