using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIS_第五章_3
{

    /// <summary>
    /// 多边形-弧段关系表
    /// </summary>
    public class PolygonAcrs
    {
        public char PolygonId;
        public List<string> ArcIds = new List<string>();
        public double Area;
    }
}
