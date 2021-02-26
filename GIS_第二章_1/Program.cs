using System;

namespace GIS_第二章_1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("输入判断点坐标X：");
            double px = double.Parse( Console.ReadLine());
            Console.WriteLine("输入判断点坐标Y：");
            double py = double.Parse(Console.ReadLine());
            Console.WriteLine("输入线段首点坐标X：");
            double sx = double.Parse(Console.ReadLine());
            Console.WriteLine("输入线段首点坐标Y：");
            double sy = double.Parse(Console.ReadLine());
            Console.WriteLine("输入线段尾点坐标X：");
            double ex = double.Parse(Console.ReadLine());
            Console.WriteLine("输入线段尾点坐标Y：");
            double ey = double.Parse(Console.ReadLine());
            Point pf = new Point(px,py);
            Point p1 = new Point(sx, sy);
            Point p2 = new Point(ex, ey);
            bool result = GetPointIsInLine(pf,p1,p2,0);
            if (result)
            { Console.WriteLine("点在线段上"); }
            else { Console.WriteLine("点不在线段上"); }
        }
        //判断点是否在直线上

        public static bool GetPointIsInLine(Point pf, Point p1, Point p2, double range)
        {

            //range 判断的的误差，不需要误差则赋值0
            //点在线段首尾两端之外则return false
            double cross = (p2.X - p1.X) * (pf.X - p1.X) + (p2.Y - p1.Y) * (pf.Y - p1.Y);
            if (cross <= 0) return false;
            double d2 = (p2.X - p1.X) * (p2.X - p1.X) + (p2.Y - p1.Y) * (p2.Y - p1.Y);
            if (cross >= d2) return false;
            double r = cross / d2;
            double px = p1.X + (p2.X - p1.X) * r;
            double py = p1.Y + (p2.Y - p1.Y) * r;
            //判断距离是否小于误差
            return Math.Sqrt((pf.X - px) * (pf.X - px) + (py - pf.Y) * (py - pf.Y)) <= range;
        }
    }
}
