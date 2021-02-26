using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIS_第五章_3
{
    public class Arc
    {
        public string Id;
        public List<Point> Points = new List<Point>();
        public int TimesOfSearch; //遍历次数
        public int Direction; //弧段的方向
    }
}
