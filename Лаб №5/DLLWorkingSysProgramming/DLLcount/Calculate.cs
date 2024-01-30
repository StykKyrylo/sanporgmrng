using System;
using System.Diagnostics;
using System.Threading;

namespace DLLcount
{
    public class Calculate
    {
        public int[] Array(int[] array, int left, int right)
        {
            Structure a = new Structure();

            a.arr = array;
            a.left = left;
            a.right = right;

            Thread myThread = new Thread(new ParameterizedThreadStart(qSort));
            myThread.Start(a);
            Thread.Sleep(15000);
            return array;
        }
        public static void Swap(ref int a, ref int b)
        {
            int tmp = a;
            a = b;
            b = tmp;
        }

        private static int Piece(int[] array, int left, int right)
        {
            int origin = array[right];
            int i = (left - 1);
            for (int j = left; j <= right - 1; j++)
                if (array[j] <= origin)
                {
                    i++;
                    Swap(ref array[i], ref array[j]);
                }
            Swap(ref array[i + 1], ref array[right]);
            return i + 1;
        }

        public struct Structure
        {
            public int left;
            public int right;
            public int[] arr;
            public int index;
        }


        public static void qSort(object ar)
        {
            Structure b = (Structure)ar;
            if (b.left < b.right)
            {

                b.index = Piece(b.arr, b.left, b.right);
                Structure buff1 = b;
                Structure buff2 = b;
                buff1.right = b.index - 1;

                Thread t1 = new Thread(new ParameterizedThreadStart(qSort));
                t1.Start(buff1);
                buff2.left = b.index + 1;

                Thread t2 = new Thread(new ParameterizedThreadStart(qSort));
                t2.Start(buff2);
                Console.WriteLine($"\n{Process.GetCurrentProcess().Threads.Count} ");
                Thread.Sleep(10);
            }
        }

    }
}
