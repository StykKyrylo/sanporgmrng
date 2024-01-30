using System;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Text;
using System.Threading;

namespace Program1
{
    class Program
    {
        static byte size = 30;
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;
            MemoryMappedFile memory = MemoryMappedFile.CreateFromFile(@"I:\Sasha\3 курс\2 семестр\Системне програмування\Lab01\data.dat",
                FileMode.OpenOrCreate, "Numbers", size);
            Semaphore semaphore = new Semaphore(3, 3, "NumbSem");
            var accessor = memory.CreateViewAccessor();
            var stream = memory.CreateViewStream();
            byte[] numbersToWrite = new byte[size];
            Random random = new Random();
            string arr = "";
            for (int i = 0; i < numbersToWrite.Length; i++)
            {
                numbersToWrite[i] = (byte)random.Next(10, 100);
                arr += numbersToWrite[i] + " ";
                Console.Write(numbersToWrite[i] + " ");
            }
            try
            {
                semaphore.WaitOne();
                accessor.WriteArray(0, numbersToWrite, 0, numbersToWrite.Length);
            }
            finally
            {
                semaphore.Release();
            }

            Process proccess2 = new Process();
            Process proccess3 = new Process();
            proccess2.StartInfo.FileName = "I:\\Sasha\\3 курс\\2 семестр\\Системне програмування\\Lab01\\Program2\\bin\\Debug\\Program2.exe";
            proccess3.StartInfo.FileName = "I:\\Sasha\\3 курс\\2 семестр\\Системне програмування\\Lab01\\Program3\\bin\\Debug\\Program3.exe";
            proccess2.Start();
            proccess3.Start();

            Console.WriteLine("\nНатиснiть пробіл для сортування(не закривайте додаток Program2, що відкрився)");
            while (Console.ReadKey().Key != ConsoleKey.Spacebar)
            {
                Console.WriteLine("Помилка! Натисніть, будь ласка, пробіл");
            }
            Console.Clear();
            Console.WriteLine(arr);
            Console.WriteLine("Зачекайте, йде сортування...");

            var handle = stream.SafeMemoryMappedViewHandle;
            unsafe
            {
                byte* pointer = null;
                handle.AcquirePointer(ref pointer);

                var d = size / 2;
                while (d >= 1)
                {
                    for (var i = d; i < size; i++)
                    {
                        try
                        {
                            semaphore.WaitOne();
                            var j = i;
                            while ((j >= d) && (*(pointer + j - d) > *(pointer + j)))
                            {
                                Swap(ref *(pointer + j), ref *(pointer + j - d));
                                j = j - d;
                            }
                        }
                        finally
                        {
                            semaphore.Release();
                        }
                        Thread.Sleep(100);
                    }

                    d = d / 2;
                }
            }
            Console.WriteLine("Сортування завершене, щоб закрити всi додатки натиснiть Enter");
            Console.ReadLine();
            try
            {
                proccess2.Kill();
            }
            catch
            {
                Console.WriteLine("Додаток 2 вимкнено");
            }
            try
            {
                proccess3.Kill();
            }
            catch
            {
                Console.WriteLine("Додаток 3 вимкнено");
            }
            Console.WriteLine("Робота додатку завершена");
        }
        static void Swap(ref byte a, ref byte b)
        {
            var t = a;
            a = b;
            b = t;
        }
    }
}