using System;
using System.Collections.Generic;
using System.Text;

namespace GIS_第十一章_2
{
    public class VertexNode
    {
        /// <summary>
        /// 获取或设置顶点的名字
        /// </summary>
        public string VertexName { get; set; }

        /// <summary>
        /// 获取或设置顶点是否被访问
        /// </summary>
        public bool Visited { get; set; }

        /// <summary>
        /// 获取或设置顶点的第一个邻接点
        /// </summary>
        public EdgeNode FirstNode { get; set; }

        /// <summary>
        /// 初始化VertexNode类的新实例
        /// </summary>
        /// <param name="vName">顶点的名字</param>
        /// <param name="firstNode">顶点的第一个邻接点</param>
        public VertexNode(string vName, EdgeNode firstNode = null)
        {
            VertexName = vName;
            Visited = false;
            FirstNode = firstNode;
        }
    }
}
