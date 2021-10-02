using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HelpDesk.Client
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            _files.Clear();
        }

        private void DropBox_Drop(object sender, DragEventArgs e)
        {

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string filePath in files)
                {
                    if (!_files.Contains(filePath))
                    {
                        _files.Add(filePath);
                    }
                }
            }

            var listbox = sender as ListBox;
            listbox.Background = new SolidColorBrush(Color.FromRgb(226, 226, 226));
        }

        private void DropBox_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
                var listbox = sender as ListBox;
                listbox.Background = new SolidColorBrush(Color.FromRgb(155, 155, 155));
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void DropBox_DragLeave(object sender, DragEventArgs e)
        {
            var listbox = sender as ListBox;
            listbox.Background = new SolidColorBrush(Color.FromRgb(226, 226, 226));
        }

        private void ListBoxAddFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = @"C:\";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Title = "Browse Text Files";
            openFileDialog.Filter = "All files (*.*)|*.*";
            openFileDialog.FilterIndex = 2;
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;
            openFileDialog.Multiselect = true;
            openFileDialog.ShowDialog();
            foreach (String file in openFileDialog.FileNames)
            {
                if (!_files.Contains(file))
                {
                    _files.Add(file);
                    dropBoxFile.Background = new SolidColorBrush(Color.FromRgb(226, 226, 226));
                }
            }
        }

        private void ListBoxAddImageFromClipboard(object sender, RoutedEventArgs e)
        {
            if (Clipboard.GetImage() != null)
            {
                string picturesSaveDir = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + @"\" + DateTime.Now.ToString("dd.MM.yyyy");
                if (!Directory.Exists(picturesSaveDir))
                {
                    Directory.CreateDirectory(picturesSaveDir);
                }
                picturesSaveDir += @"\" + DateTime.Now.ToString("dd.MM.yyyy_mmHHss") + ".jpeg";
                SaveClipboardImageToFile(picturesSaveDir);
                if (!_files.Contains(picturesSaveDir))
                {
                    _files.Add(picturesSaveDir);
                    dropBoxFile.Background = new SolidColorBrush(Color.FromRgb(226, 226, 226));
                }
            }
        }

        private void ListBoxDelFile(object sender, RoutedEventArgs e)
        {
            for (int x = dropBoxFile.SelectedItems.Count - 1; x >= 0; x--)
            {
                _files.Remove(dropBoxFile.SelectedItems[x].ToString());
            }
        }

        public static void SaveClipboardImageToFile(string filePath)
        {
            BitmapSource image = Clipboard.GetImage();
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image));
                encoder.Save(fileStream);
            }
        }

        private void ListBoxOpenFile(object sender, RoutedEventArgs e)
        {
            for (int x = dropBoxFile.SelectedItems.Count - 1; x >= 0; x--)
            {
                string partFile = dropBoxFile.SelectedItems[x].ToString();
                if (System.IO.File.Exists(partFile))
                {
                    System.Diagnostics.Process.Start(partFile);
                }
            }
        }

        private void dropBoxFile_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            for (int x = dropBoxFile.SelectedItems.Count - 1; x >= 0; x--)
            {
                string partFile = dropBoxFile.SelectedItems[x].ToString();
                if (System.IO.File.Exists(partFile))
                {
                    System.Diagnostics.Process.Start(partFile);
                }
            }
        }

        private ObservableCollection<string> _files = new ObservableCollection<string>();

    }
}
