using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;



namespace DiskInfo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            listView1.View = View.Details;

            DriveInfo[] drives = DriveInfo.GetDrives(); // створення масиву drives, в який буде записуватись інформація про диски
            for (int i = 0; i < drives.Length; i++) // перебираємо усі елементи масиву, тобто диски
            {
                string freeSpace, totalSize, format, volume, type; //створюємо змінні, в які буде записуватись детальна інформація про кожен з дисків
                try //коли нам відома інформація
                {
                    freeSpace = drives[i].AvailableFreeSpace.ToString();
                    format = drives[i].DriveFormat;
                    totalSize = drives[i].TotalSize.ToString();
                    volume = drives[i].VolumeLabel.ToString();

                }
                catch //коли невідома інформація
                {
                    freeSpace = "Невідомо";
                    format = "Невідомо";
                    totalSize = "Невідомо";
                    volume = "Невідомо";
                }

                type = drives[i].DriveType.ToString();

                columnHeader1.ListView.Items.Add(drives[i].Name);
                columnHeader2.ListView.Items[i].SubItems.Add(type);
                columnHeader3.ListView.Items[i].SubItems.Add(freeSpace);
                columnHeader4.ListView.Items[i].SubItems.Add(totalSize);
                columnHeader5.ListView.Items[i].SubItems.Add(format);
                columnHeader6.ListView.Items[i].SubItems.Add(volume);
            }
            var allMemory = new Microsoft.VisualBasic.Devices.ComputerInfo().TotalPhysicalMemory;
            label1.Text = "Кількість оперативної пам'яті (в байтах) = " + allMemory.ToString();
            label2.Text = "Імя комп'ютера: " + Environment.MachineName;
            label3.Text = "Імя користувача: " + Environment.UserName;
            label4.Text = "Шлях до системної директорії: " + Environment.SystemDirectory;
            label5.Text = "Шлях до директорії, відведеної для тимчасових файлів: " + Path.GetTempPath();
            label6.Text = "Шлях до Windows директорії: " + Environment.GetEnvironmentVariable("SystemRoot");
            label7.Text = "Поточна директорія: " + Directory.GetCurrentDirectory();
        }
        public void Run()
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = @"C:\Users\kp611\Desktop";
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
               | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            watcher.Filter = "*.txt";
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.Created += new FileSystemEventHandler(OnChanged);
            watcher.Deleted += new FileSystemEventHandler(OnChanged);
            watcher.Renamed += new RenamedEventHandler(OnRenamed);
            watcher.EnableRaisingEvents = true;
            File.Delete(@"D:\Лабы\Системне та мережне програмування\Лаб №3\log.txt");
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            string path = @"D:\Лабы\Системне та мережне програмування\Лаб №3\log.txt";
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine($"File: {e.FullPath} {e.ChangeType}");
            }
        }

        private void OnRenamed(object source, RenamedEventArgs e)
        {
            string path = @"D:\Лабы\Системне та мережне програмування\Лаб №3\log.txt";

            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine($"File: {e.OldFullPath} renamed to {e.FullPath}");
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Run();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}