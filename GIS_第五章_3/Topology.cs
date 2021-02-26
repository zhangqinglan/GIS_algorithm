using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GIS_第五章_3
{
    class Topology
    {
        public IList<Point> Points = new List<Point>();
        public IList<Arc> Arcs = new List<Arc>();
        public IList<ArcPoints> ArcPointses = new List<ArcPoints>();
        public IList<PointArcs> PointArcses = new List<PointArcs>();
        public IList<PolygonAcrs> PolygonAcrses = new List<PolygonAcrs>();
        public IList<ArcPolygon> ArcPolygons = new List<ArcPolygon>();
        public Topology(IList<Point> points)
        {
            this.Points = points;
        }
        public void BuildArcPoints(IList<Arc> arcs)
        {
            foreach (var arc in arcs)
            {
                var arcPoints = new ArcPoints { ArcId = arc.Id };
                foreach (var point in arc.Points)
                {
                    arcPoints.PointIds.Add(point.Id);
                }
                ArcPointses.Add(arcPoints);
            }
        }
        public void BuildPolygonsArcs(IList<Polygon> polygons)
        {
            foreach (var polygon in polygons)
            {
                var polygonAcrs = new PolygonAcrs { PolygonId = polygon.Id, Area = polygon.Area };
                foreach (var arcDirection in polygon.ArcDirections)
                {
                    polygonAcrs.ArcIds.Add(arcDirection.Arc.Id);
                }
                //单弧段多边形
                //if (polygon.ArcDirections.Count == 1)
                //{
                //    if(PointInIsland(polygons,polygon))
                //}
                PolygonAcrses.Add(polygonAcrs);
            }
        }
        public bool PointInIsland(IList<Polygon> polygons, Polygon polygon)
        {
            var point = polygon.ArcDirections[0].Arc.Points[0];
            return (from p in polygons where polygon.Id != p.Id select p.Points.Contains(point)).FirstOrDefault();
        }

        public void BuildArcsPolygon(IList<Arc> arcs, IList<Polygon> polygons)
        {
        }
        public void PrintTopology(string outpath)
        {
            string topology = "点结构表" + "\n" + "点ID:横坐标,纵坐标"+"\n";
            foreach (var point in Points)
            {
                string id_coor = point.Id + ":" + point.X +","+ point.Y+"\n";
                topology = topology + id_coor;
            }
            string topology_1 = "段-点拓扑结构表" + "\n" + "段ID：点ID" + "\n";
            topology = topology + topology_1;
            //Console.WriteLine();
            //Console.WriteLine("段-点拓扑结构表");
            //Console.WriteLine("段ID：点ID");
            foreach (var arcPointses in ArcPointses)
            {
                var pointIds = arcPointses.PointIds.Aggregate("", (current, pId) => current + (pId + ","));
                string arc_p = arcPointses.ArcId + ":" + pointIds.TrimEnd(',') + "\n";
                topology = topology + arc_p;
                //Console.WriteLine("{0}:{1}", arcPointses.ArcId, pointIds.TrimEnd(','));
            }
            string topology_2 = "多边形-段拓扑结构表" + "\n" + "多边形ID：面积；段ID" + "\n";
            topology = topology + topology_2;
            //Console.WriteLine();
            //Console.WriteLine("多边形-段拓扑结构表");
            //Console.WriteLine("多边形ID：面积；段ID");
            foreach (var polygonAcrses in PolygonAcrses)
            {
                var arcIds = polygonAcrses.ArcIds.Aggregate("", (current, aId) => current + (aId + ","));
                string polygon_arc = polygonAcrses.PolygonId + ":" + polygonAcrses.Area + arcIds.TrimEnd(',');
                topology = topology + polygon_arc;
                //Console.WriteLine("{0}:{1};{2}", polygonAcrses.PolygonId, polygonAcrses.Area, arcIds.TrimEnd(','));
            }
            File.WriteAllText(outpath, topology);
        }
    }
}
