using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIS_第五章_3
{
    /// <summary>
    /// 弧段-多边形关系表
    /// </summary>
    public class ArcPolygon
    {
        public string ArcId;
        public int StartPointId;
        public int EndPointId;
        public char LeftPolygonId;
        public char RightPolygonId;
    }
}
