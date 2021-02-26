using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIS_第五章_3
{
    /// <summary>
    /// 多边形模型
    /// </summary>
    public class Polygon
    {
        public char Id;
        public List<Point> Points = new List<Point>();
        public List<ArcDirection> ArcDirections = new List<ArcDirection>();
        public double Area;
    }
}
