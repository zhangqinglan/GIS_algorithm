//Dijkstra算法程序实现单点源最短路径的计算
using System;

namespace GIS_第十一章_1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            int r;     //列数
            Console.Write("请输入点个数（含配送中心点）: ");
            Int32.TryParse(Console.ReadLine(), out r);
            Console.WriteLine("各点分别为: \n");
            for (int i = 0; i < r; i++)
                Console.Write("V{0} ", i);
            Console.Write("  假定第一个点是配送中心");
            Console.WriteLine("\n\n输入各点之间的距离(无通径的用个大整数表示)\n");

            int[] a = new int[r * r];
            int da;

            for (int i = 0; i < r; i++)
            {
                for (int j = i + 1; j < r; j++)
                {
                    Console.Write("V{0} 到 V{1}的距离是:  ", i, j);
                    Int32.TryParse(Console.ReadLine(), out da);
                    a[i * r + j] = da;
                    Console.WriteLine();
                }
            }
            //----完善距离矩阵(距离矩阵其实可以是个上三角矩阵，
            //----但为了处理方便，还是将其完整成一个对称阵)-----------
            for (int i = 0; i < r; i++)
            {
                for (int j = 0; j < r; j++)
                {
                    if (i == j)
                    {
                        a[i * r + j] = 0;
                    }
                    a[j * r + i] = a[i * r + j];
                }
            }
            Marx m = new Marx(r, a);
            Console.WriteLine();
            m.Find_way();
            m.Display();
        }
    }
}
