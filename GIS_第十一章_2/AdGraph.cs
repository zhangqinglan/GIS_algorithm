using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace GIS_第十一章_2
{
    public class AdGraph
    {
        private readonly VertexNode[] _vertexList; //结点表

        /// <summary>
        /// 获取图的结点数
        /// </summary>
        public int VertexCount { get; }

        /// <summary>
        /// 初始化AdGraph类的新实例
        /// </summary>
        /// <param name="vCount">图中结点的个数</param>
        public AdGraph(int vCount)
        {
            if (vCount <= 0)
                throw new ArgumentOutOfRangeException();

            VertexCount = vCount;
            _vertexList = new VertexNode[vCount];
        }

        /// <summary>
        /// 获取或设置图中各结点的名称
        /// </summary>
        /// <param name="index">结点名称从零开始的索引</param>
        /// <returns>指定索引处结点的名称</returns>
        public string this[int index]
        {
            get
            {
                if (index < 0 || index > VertexCount - 1)
                    throw new ArgumentOutOfRangeException();

                return _vertexList[index] == null
                    ? "NULL"
                    : _vertexList[index].VertexName;
            }
            set
            {
                if (index < 0 || index > VertexCount - 1)
                    throw new ArgumentOutOfRangeException();

                if (_vertexList[index] == null)
                    _vertexList[index] = new VertexNode(value);
                else
                    _vertexList[index].VertexName = value;
            }
        }

        /// <summary>
        /// 得到结点在结点表中的位置
        /// </summary>
        /// <param name="vertexName">结点的名称</param>
        /// <returns>结点的位置</returns>
        private int GetIndex(string vertexName)
        {
            int i;
            for (i = 0; i < VertexCount; i++)
            {
                if (_vertexList[i] != null && _vertexList[i].VertexName == vertexName)
                    break;
            }
            return i == VertexCount ? -1 : i;
        }

        /// <summary>
        /// 给图加边
        /// </summary>
        /// <param name="startVertexName">起始结点的名字</param>
        /// <param name="endVertexName">终止结点的名字</param>
        /// <param name="weight">边上的权值</param>
        public void AddEdge(string startVertexName, string endVertexName
            , double weight = 0.0)
        {
            int i = GetIndex(startVertexName);
            int j = GetIndex(endVertexName);

            if (i == -1 || j == -1)
                throw new Exception("图中不存在该边.");

            EdgeNode temp = _vertexList[i].FirstNode;
            if (temp == null)
            {
                _vertexList[i].FirstNode = new EdgeNode(j, weight);
            }
            else
            {
                while (temp.Next != null)
                    temp = temp.Next;
                temp.Next = new EdgeNode(j, weight);
            }
        }
        /// <summary>
        /// Prim算法 最小生成树
        /// </summary>
        /// <returns></returns>
        public SpanTreeNode[] MiniSpanTree(string vName)
        {
            int i = GetIndex(vName);
            if (i == -1)
                return null;

            SpanTreeNode[] spanTree = new SpanTreeNode[VertexCount];

            //首先加入根节点
            spanTree[0] = new SpanTreeNode(_vertexList[i].VertexName,
                "NULL", 0.0);

            //U中结点到各结点最小权值那个结点在VertexList中的索引号
            int[] vertexIndex = new int[VertexCount];

            //U中结点到各个结点的最小权值
            double[] lowCost = new double[VertexCount];

            for (int j = 0; j < VertexCount; j++)
            {
                lowCost[j] = double.MaxValue;
                vertexIndex[j] = i;
            }

            EdgeNode p1 = _vertexList[i].FirstNode;
            while (p1 != null)
            {
                lowCost[p1.Index] = p1.Weight;
                p1 = p1.Next;
            }
            vertexIndex[i] = -1;

            for (int count = 1; count < VertexCount; count++)
            {
                double min = double.MaxValue;
                int v = i;
                for (int k = 0; k < VertexCount; k++)
                {
                    if (vertexIndex[k] != -1 && lowCost[k] < min)
                    {
                        min = lowCost[k];
                        v = k;
                    }
                }
                spanTree[count] = new SpanTreeNode(_vertexList[v].VertexName,
                    _vertexList[vertexIndex[v]].VertexName, min);
                vertexIndex[v] = -1;

                EdgeNode p2 = _vertexList[v].FirstNode;
                while (p2 != null)
                {
                    if (vertexIndex[p2.Index] != -1 &&
                        p2.Weight < lowCost[p2.Index])
                    {
                        lowCost[p2.Index] = p2.Weight;
                        vertexIndex[p2.Index] = v;
                    }
                    p2 = p2.Next;
                }
            }
            return spanTree;
        }

        /// <summary>
        /// Kruskal算法 最小生成树
        /// </summary>
        /// <returns></returns>
        public SpanTreeNode[] MiniSpanTree()
        {
            int[] parent = new int[VertexCount];
            for (int i = 0; i < VertexCount; i++)
            {
                parent[i] = 0;
            }
            SpanTreeNode[] tree = new SpanTreeNode[VertexCount];
            int count = 0;
            Edge[] edges = GetEdges();

            for (int i = 0; i < edges.Length; i++)
            {
                int begin = edges[i].Begin;
                int end = edges[i].End;
                int n = Find(parent, begin);
                int m = Find(parent, end);
                if (n != m)
                {
                    if (i == 0)
                    {
                        tree[count] = new SpanTreeNode(_vertexList[begin].VertexName, "NULL", 0.0);
                        count++;
                    }
                    parent[n] = m;
                    tree[count] = new SpanTreeNode(_vertexList[end].VertexName,
                        _vertexList[begin].VertexName, edges[i].Weight);
                    count++;
                }
            }
            return tree;
        }
        private Edge[] GetEdges()
        {
            for (int i = 0; i < VertexCount; i++)
                _vertexList[i].Visited = false;

            List<Edge> result = new List<Edge>();

            for (int i = 0; i < VertexCount; i++)
            {
                _vertexList[i].Visited = true;
                EdgeNode p = _vertexList[i].FirstNode;
                while (p != null)
                {
                    if (_vertexList[p.Index].Visited == false)
                    {
                        Edge edge = new Edge(i, p.Index, p.Weight);
                        result.Add(edge);
                    }
                    p = p.Next;
                }
            }
            return result.OrderBy(a => a.Weight).ToArray();
        }
        private int Find(int[] parent, int f)
        {
            while (parent[f] > 0)
                f = parent[f];
            return f;
        }
        public static AdGraph CreateGraph()
        {
            AdGraph result = new AdGraph(6);
            result[0] = "V0";
            result[1] = "V1";
            result[2] = "V2";
            result[3] = "V3";
            result[4] = "V4";
            result[5] = "V5";
            result.AddEdge("V0", "V1", 6);
            result.AddEdge("V0", "V2", 1);
            result.AddEdge("V0", "V3", 5);
            result.AddEdge("V1", "V0", 6);
            result.AddEdge("V1", "V2", 5);
            result.AddEdge("V1", "V4", 3);
            result.AddEdge("V2", "V0", 1);
            result.AddEdge("V2", "V1", 5);
            result.AddEdge("V2", "V3", 7);
            result.AddEdge("V2", "V4", 5);
            result.AddEdge("V2", "V5", 4);
            result.AddEdge("V3", "V0", 5);
            result.AddEdge("V3", "V2", 7);
            result.AddEdge("V3", "V5", 2);
            result.AddEdge("V4", "V1", 3);
            result.AddEdge("V4", "V2", 5);
            result.AddEdge("V4", "V5", 6);
            result.AddEdge("V5", "V2", 4);
            result.AddEdge("V5", "V3", 2);
            result.AddEdge("V5", "V4", 6);
            return result;
        }

    }
}
