using System;
using System.Collections.Generic;
using System.Text;

namespace GIS_第十一章_2
{
    public class EdgeNode
    {
        /// <summary>
        /// 获取边终点在顶点数组中的位置
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// 获取边上的权值
        /// </summary>
        public double Weight { get; }

        /// <summary>
        /// 获取或设置下一个邻接点
        /// </summary>
        public EdgeNode Next { get; set; }

        /// <summary>
        /// 初始化EdgeNode类的新实例
        /// </summary>
        /// <param name="index">边终点在顶点数组中的位置</param>
        /// <param name="weight">边上的权值</param>
        /// <param name="next">下一个邻接点</param>
        public EdgeNode(int index, double weight = 0.0, EdgeNode next = null)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException();

            Index = index;
            Weight = weight;
            Next = next;
        }
    }
}
