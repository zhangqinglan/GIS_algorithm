using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace GIS_第十一章_1
{
    class Marx
    {
        private int[] distance;
        private int row;
        private ArrayList ways = new ArrayList();
        public Marx(int n, params int[] d)
        {
            this.row = n;
            distance = new int[row * row];
            for(int i = 0;i<row*row;i++)
            {
                this.distance[i] = d[i];
            }
            for (int i=0;i<this.row;i++)
            {
                ArrayList w = new ArrayList();
                int j = 0;
                w.Add(j);
                ways.Add(w);
            }
        }
        public void Find_way()
        {
            ArrayList S = new ArrayList(1);
            ArrayList Sr = new ArrayList(1);
            int[] Indexof_distance = new int[this.row];
            for (int i =0;i<row;i++)
            {
                Indexof_distance[i] = i;
            }
            S.Add(Indexof_distance[0]);
            for(int i=0;i<this.row;i++)
            {
                Sr.Add(Indexof_distance[i]);
            }
            Sr.RemoveAt(0);
            int[] D = new int[this.row];

            int Count = this.row - 1;
            while(Count>0)
            {
                for(int i=0;i<row;i++)
                {
                    D[i] = this.distance[i];
                }
                int min_num = (int)Sr[0];
                foreach (int s in Sr)
                {
                    if (D[s] < D[min_num]) min_num = s;
                }

                S.Add(min_num);
                Sr.Remove(min_num);

                ((ArrayList)ways[min_num]).Add(min_num);
                foreach (int element in Sr)
                {
                    int position = element * (this.row) + min_num;
                    bool exchange = false;      //有交换标志
                    if (D[element] < D[min_num] + this.distance[position])
                        D[element] = D[element];
                    else
                    {
                        D[element] = this.distance[position] + D[min_num];
                        exchange = true;
                    }
                    //修改距离矩阵                  
                    this.distance[element] = D[element];
                    position = element * this.row;
                    this.distance[position] = D[element];
                    //修改路径---------------
                    if (exchange == true)
                    {
                        ((ArrayList)ways[element]).Clear();
                        foreach (int point in (ArrayList)ways[min_num])
                            ((ArrayList)ways[element]).Add(point);
                    }
                }
                --Count;
            }
        }
        public void Display()
        {
            //------中心到各点的最短路径----------
            Console.WriteLine("中心到各点的最短路径如下: \n\n");
            int sum_d_index = 0;
            foreach (ArrayList mother in ways)
            {
                foreach (int child in mother)
                    Console.Write("V{0} -- ", child + 1);
                Console.WriteLine("    路径长 {0}", distance[sum_d_index++]);
            }
        }
    }
}
