using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Lab4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public void refresh()
        {
            RegistryKey k = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            RegistryKey keyMachine = Registry.LocalMachine.OpenSubKey("SOFTWARE").OpenSubKey("Microsoft").OpenSubKey("Windows").OpenSubKey("CurrentVersion").OpenSubKey("Run", true);
            RegistryKey keyUser = Registry.CurrentUser.OpenSubKey("SOFTWARE").OpenSubKey("Microsoft").OpenSubKey("Windows").OpenSubKey("CurrentVersion").OpenSubKey("Run", true);
            RegistryKey keySchedule = k.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\TaskCache\Tree\");
            string[] machine = keyMachine.GetValueNames();
            string[] user = keyUser.GetValueNames();
            string[] schedule = keySchedule.GetSubKeyNames();
            label1.Text = "Автозагрузка:\n";
            label2.Text = "Планувальник завдань: \n";
            for (int i = 0; i < machine.Length; i++)
                label1.Text += machine[i] + "\n";
            for (int i = 0; i < user.Length; i++)
                label1.Text += user[i] + "\n";
            for (int i = 0; i < schedule.Length; i++)
                label2.Text += schedule[i] + "\n";
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RegistryKey keyUser = Registry.CurrentUser.OpenSubKey("SOFTWARE").OpenSubKey("Microsoft").OpenSubKey("Windows").OpenSubKey("CurrentVersion").OpenSubKey("Run", true);
            keyUser.SetValue("notepad.exe", @"C:\Windows\system32\notepad.exe");
            refresh();
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            RegistryKey keyUser = Registry.CurrentUser.OpenSubKey("SOFTWARE").OpenSubKey("Microsoft").OpenSubKey("Windows").OpenSubKey("CurrentVersion").OpenSubKey("Run", true);
            keyUser.DeleteValue("notepad.exe");
            refresh();
        }

        private void Copy_Click(object sender, EventArgs e)
        {
            string path = @"C:\Users\kp611\Desktop\copy.reg";
            string key = @"HKEY_CURRENT_USER";
            Process proc = new Process();
            try
            {
                proc.StartInfo.FileName = "regedit.exe";
                proc.StartInfo.UseShellExecute = false;
                proc = Process.Start("regedit.exe", "/e " + path + " " + key);
                proc.WaitForExit();
            }
            catch (Exception)
            {
                proc.Dispose();
            }
        }

        private void ttt_Click(object sender, EventArgs e)
        {
            var path = $@"C:\Users\kp611\Desktop\ttt.reg";
            string arguments = $"/s {path}";
            var regeditProcess = Process.Start($"regedit.exe", arguments);
            regeditProcess.WaitForExit();
        }
    }
}
