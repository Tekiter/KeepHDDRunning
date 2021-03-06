﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace KeepHDDRunning
{
    public class Folder
    {
        public bool Exists => info.Exists;
        public string Path => info.FullName;
        public string Name { get; set; }
        public object Icon => Imaging.CreateBitmapSourceFromHIcon(GetLargeIcon(Path).Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

        public FileAttributes Attributes => info.Attributes; 

        DirectoryInfo info;

        public Folder(string path)
        {
            info = new DirectoryInfo(path);
            Name = info.Name;
        }

        public override string ToString()
        {
            return info.Name;
        }


        #region GetIcon
        [StructLayout(LayoutKind.Sequential)]
        private struct SHFILEINFO
        {
            public IntPtr hIcon;
            public IntPtr iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        };
        private const uint SHGFI_ICON = 0x100;
        private const uint SHGFI_LARGEICON = 0x0;
        private const uint SHGFI_SMALLICON = 0x1;

        [DllImport("shell32.dll")]
        private static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);

        [DllImport("User32.dll")]
        private static extern int DestroyIcon(IntPtr hIcon);

        private Icon GetSmallIcon(string fileName)
        {
            return GetIcon(fileName, SHGFI_SMALLICON);
        }

        private Icon GetLargeIcon(string fileName)
        {
            return GetIcon(fileName, SHGFI_LARGEICON);
        }

        private Icon GetIcon(string fileName, uint flags)
        {
            SHFILEINFO shinfo = new SHFILEINFO();
            IntPtr hImgSmall = SHGetFileInfo(fileName, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), SHGFI_ICON | flags);

            Icon icon = (Icon)System.Drawing.Icon.FromHandle(shinfo.hIcon).Clone();
            DestroyIcon(shinfo.hIcon);
            return icon;
        }
        #endregion
    }
}
