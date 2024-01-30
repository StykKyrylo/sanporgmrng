using System;
using System.IO.MemoryMappedFiles;
using System.Threading;
using System.Windows.Forms;

namespace Program2
{
    public partial class Form1 : Form
    {
        static byte size = 30;
        MemoryMappedFile memory = MemoryMappedFile.OpenExisting("Numbers");
        Semaphore semaphore = Semaphore.OpenExisting("NumbSem");

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Render();
            timer1.Enabled = true;
            timer1.Start();
        }
        public void Render()
        {

            try
            {
                semaphore.WaitOne();

                string res = "";
                var stream = memory.CreateViewStream();
                var handle = memory.CreateViewStream().SafeMemoryMappedViewHandle;
                unsafe
                {
                    byte* pointer = null;
                    handle.AcquirePointer(ref pointer);

                    for (int i = 0; i < size; i++)
                    {
                        for (int j = 0; j < *(pointer + i); j++)
                            res += "*";
                        res += $" {*(pointer + i)}";
                        res += "\n";
                    }
                    label1.Text = res;
                }
            }
            finally
            {
                semaphore.Release();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Render();
        }
    }
}