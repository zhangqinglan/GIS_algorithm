using System;

namespace GIS_第十一章_2
{
    class Program
    {
        static void Main(string[] args)
        {
            AdGraph alg = AdGraph.CreateGraph();
            SpanTreeNode[] tree = alg.MiniSpanTree("V2");
            double sum = 0;
            for (int i = 0; i < tree.Length; i++)
            {
                string str = "(" + tree[i].ParentName + ","
                                + tree[i].SelfName + ") Weight:"
                                + tree[i].Weight;
                Console.WriteLine(str);
                sum += tree[i].Weight;
            }
            Console.WriteLine(sum);
            //Kruskal算法生成结果
            SpanTreeNode[] tree1 = alg.MiniSpanTree();
            double sum1 = 0;
            for (int i = 0; i < tree1.Length; i++)
            {
                string str = "(" + tree[i].ParentName + ","
                                + tree[i].SelfName + ") Weight:"
                                + tree[i].Weight;
                Console.WriteLine(str);
                sum1 += tree[i].Weight;
            }
            Console.WriteLine(sum1);

        }
    }
}
