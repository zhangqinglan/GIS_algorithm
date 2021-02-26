using System;
using System.Collections.Generic;
using System.Text;

namespace GIS_第十一章_2
{
    internal class Edge
    {
        /// <summary>
        /// 起点编号
        /// </summary>
        public int Begin { get; }

        /// <summary>
        /// 终点编号
        /// </summary>
        public int End { get; }

        /// <summary>
        /// 权值
        /// </summary>
        public double Weight { get; }

        /// <summary>
        /// 创建一个 Edge 类的新实例
        /// </summary>
        /// <param name="begin">起点编号</param>
        /// <param name="end">终点编号</param>
        /// <param name="weight">权值</param>

        public Edge(int begin, int end, double weight = 0.0)
        {
            Begin = begin;
            End = end;
            Weight = weight;
        }


    }
}
