using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
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
using Microsoft.WindowsAPICodePack.Dialogs;

namespace YouTube
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string folder;
        public MainWindow()
        {
            InitializeComponent();
            comboBox_quality.SelectedIndex = comboBox_quality.Items.Count - 1;
            comboBox_extension.SelectedIndex = 0;
            folder = Environment.CurrentDirectory;
            textBox_folder.Text = folder;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            string cmd = strCmd();
            if(cmd != string.Empty)
            {
                System.Diagnostics.Process.Start("CMD.exe", "/K " + cmd);
            }
            else
            {
                MessageBox.Show("Что-то пошло не так");
            }
        }

        private string strCmd()
        {
            string url = textBox_url.Text;
            string quality = comboBox_quality.Text.TrimEnd('p');
            string extension = comboBox_extension.Text;
            if (checkBox_range.IsChecked == true)
            {
                string start = textBox_start.Text;
                string stop = textBox_stop.Text;
                if (int.TryParse(start, out var res_start) || start == string.Empty)
                {
                    if (int.TryParse(stop, out var res_stop) || stop == string.Empty)
                    {
                        return "yt-dlp.exe -f \"bv[height=" + quality + "][ext=" + extension + "]\" -I \"" + start + ":" + stop + "\" " + "-P " + folder +" "+ url;
                    }
                    else return string.Empty;
                }else return string.Empty;
            }
            else
            {
                return "yt-dlp.exe -f \"bv[height=" + quality + "][ext=" + extension + "]\" " + "-P " + folder + " " + url;
            }
            
        }

        private void button_clear_Click(object sender, RoutedEventArgs e)
        {
            textBox_url.Text = string.Empty;
            
        }

        private void button_folder_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new CommonOpenFileDialog();
            dlg.IsFolderPicker = true;
            if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
            {
                if(dlg.FileName != string.Empty)
                {
                    folder = dlg.FileName;
                    textBox_folder.Text = folder;
                }
            }
        }
    }
}
