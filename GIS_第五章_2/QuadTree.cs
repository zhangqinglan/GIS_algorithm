using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIS_第五章_2
{
    public class QuadTree
    {
        public QuadTree()
        {
            RootNode = null;
            Nodes = new List<QNode>();
        }
        public QuadTree(QNode pRootNode)
        {
            RootNode = pRootNode;
        }
        public QNode RootNode;
        public List<QNode> Nodes;

    }
}
