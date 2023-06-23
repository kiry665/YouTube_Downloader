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
using YoutubeDLSharp;
using static System.Windows.Forms.DataFormats;

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

        private void button_download_Click(object sender, RoutedEventArgs e)
        {
            string cmd = strCmd();
            if(cmd != string.Empty )
            {
                Process.Start("CMD.exe", "/K " + cmd);
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
                if (dlg.FileName != string.Empty)
                {
                    folder = dlg.FileName;
                    textBox_folder.Text = folder;
                }
            }
        }

        private string strCmd()
        {
            string url = textBox_url.Text;
            if (url == string.Empty)
            {
                MessageBox.Show("Нет ссылки на видео или плейлист");
                return string.Empty;
            }
            else
            {
                if (radioButton_video.IsChecked == true)
                {
                    if (listBox_formats.SelectedItem != null)
                    {
                        string format = listBox_formats.SelectedItem.ToString().Split().First();
                        return "yt-dlp.exe -f \"(" + format + ")\" -P " + folder + " " + url;
                    }
                    else
                    {
                        getInfo();
                        MessageBox.Show("Выберите элемент из списка");
                        return string.Empty;
                    }
                }
                else 
                {
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
                                return "yt-dlp.exe -f \"bestvideo[height=" + quality + "][ext=" + extension + "]\" -I \"" + start + ":" + stop + "\" " + "-P " + folder + " " + url;
                            }
                            else return string.Empty;
                        }
                        else return string.Empty;
                    }
                    else
                    {
                        return "yt-dlp.exe -f \"bestvideo[height=" + quality + "][ext=" + extension + "]\" " + "-P " + folder + " " + url;
                    }
                }
                
            }
        }

        private void getInfo()
        {
            listBox_formats.Items.Clear();
            string url = textBox_url.Text;
            var startInfo = new ProcessStartInfo("cmd.exe", "/C yt-dlp.exe -F " + url);
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            startInfo.RedirectStandardOutput = true;

            var proc = new Process();
            proc.StartInfo = startInfo;
            proc.Start();

            var result = proc.StandardOutput.ReadToEnd();
            proc.WaitForExit();
            //textBox_info.Text = result;

            string[] lines = result.Split(
                new string[] { "\n" },
                StringSplitOptions.None
                );

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("mp4") && lines[i].Contains("video"))
                {
                    listBox_formats.Items.Add(lines[i]);//.Remove(0, lines[i].IndexOf(' ') + 1));
                }
            }
        }

        private void buttonInfo_Click(object sender, RoutedEventArgs e)
        {
            getInfo();
        }

        private void listBox_formats_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
