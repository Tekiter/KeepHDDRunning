using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KeepHDDRunning
{
    /// <summary>
    /// SelectFolderDialog.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SelectFolderDialog : Window
    {
        public ObservableCollection<Folder> FolderList { get; set; }
        string CurrentLocation { get; set; }

        public string SelectedFolder { get; set; }

        public SelectFolderDialog()
        {
            InitializeComponent();
            DataContext = this;
            FolderList = new ObservableCollection<Folder>();
            CurrentLocation = "";
            //Refresh();
        }

        public SelectFolderDialog(string initialPath) : this()
        {
            CurrentLocation = initialPath;
        }
        

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(SelectedFolder))
            {
                CurrentLocation = SelectedFolder;
                Txt_Selected.Text = SelectedFolder;
            }
            Refresh();
        }



        public void Refresh()
        {
            FolderList.Clear();
            Txt_Selected.Text = CurrentLocation;
            if (string.IsNullOrWhiteSpace(CurrentLocation))
            {
                

                DriveInfo[] drives = DriveInfo.GetDrives();
                foreach (var i in drives)
                {
                    Folder cur = new Folder(i.Name);
                    cur.Name = string.Format("{0} ({1})", i.VolumeLabel, i.Name);
                    FolderList.Add(cur);
                }
            }
            else
            {

                try
                {
                    DirectoryInfo info = new DirectoryInfo(CurrentLocation);
                    if (info.Exists)
                    {

                        foreach (var i in info.GetDirectories())
                        {
                            Folder cur = new Folder(i.FullName);
                            if (!cur.Attributes.HasFlag(FileAttributes.Hidden))
                                FolderList.Add(cur);
                        }


                    }
                }
                catch (UnauthorizedAccessException)
                {
                    FolderList.Clear();
                }
                
            }
            
        }

        private void Txt_Selected_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
                if (Directory.Exists(Txt_Selected.Text))
                {
                    CurrentLocation = Txt_Selected.Text;
                    Refresh();
                }
        }

        private void List_Dirs_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (List_Dirs.SelectedItem is Folder fol)
            {
                CurrentLocation = fol.Path;
                Txt_Selected.Text = fol.Path;
                Refresh();
            }
        }

       

        private void List_Dirs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (List_Dirs.SelectedItem is Folder fol)
            {
                Txt_Selected.Text = fol.Path;
            }
        }

        private void Btn_New_Click(object sender, RoutedEventArgs e)
        {
            CurrentLocation = "C:\\";
            Refresh();
        }

        private void Btn_Back_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CurrentLocation))
                return;

            try
            {
                CurrentLocation = Directory.GetParent(CurrentLocation).FullName;
            }
            catch (NullReferenceException)
            {
                CurrentLocation = "";
            }
            finally
            {
                Txt_Selected.Text = CurrentLocation;
                Refresh();
            }
            
        }
        
        private void OK_Click(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(Txt_Selected.Text))
            {
                DialogResult = true;
                SelectedFolder = Txt_Selected.Text;
                Close();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }


        #region DisableCloseButton
        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        private const int GWL_STYLE = -16;
        private const int WS_MINIMIZEBOX = 0x20000;

        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            IntPtr handle = new WindowInteropHelper(this).Handle;
            if (handle != null)
            {
                SetWindowLong(handle, GWL_STYLE, GetWindowLong(handle, GWL_STYLE) & ~WS_MINIMIZEBOX);
            }
        }
        #endregion


    }

    
}
