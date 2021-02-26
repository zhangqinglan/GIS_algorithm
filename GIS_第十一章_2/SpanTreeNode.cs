using System;
using System.Collections.Generic;
using System.Text;

namespace GIS_第十一章_2
{
    public class SpanTreeNode
    {
        /// <summary>
        /// 获取或设置结点本身的名称
        /// </summary>
        public string SelfName { get; }

        /// <summary>
        /// 获取或设置结点双亲的名称
        /// </summary>
        public string ParentName { get; }

        /// <summary>
        /// 获取或设置边的权值
        /// </summary>
        public double Weight { get; set; }

        /// <summary>
        /// 构造SpanTreeNode实例
        /// </summary>
        /// <param name="selfName">结点本身的名称</param>
        /// <param name="parentName">结点双亲的名称</param>
        /// <param name="weight">边的权值</param>
        public SpanTreeNode(string selfName, string parentName, double weight)
        {
            if (string.IsNullOrEmpty(selfName) || string.IsNullOrEmpty(parentName))
                throw new ArgumentNullException();

            SelfName = selfName;
            ParentName = parentName;
            Weight = weight;
        }


    }
}
