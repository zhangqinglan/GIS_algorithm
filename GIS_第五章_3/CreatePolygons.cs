using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIS_第五章_3
{
    public class CreatePolygons
    {
        public IList<Point> Points = new List<Point>();
        public IList<Arc> Arcs = new List<Arc>();
        public IList<PointArcs> PointArcses = new List<PointArcs>();
        public IList<Polygon> Polygons = new List<Polygon>();
        public int PolygonIndex;
        public ArcAngle MinAngle;
        public CreatePolygons(IList<Point> points, IList<Arc> arcs)
        {
            Points = points;
            Arcs = arcs;
        }

        public void MainCreatePolygons()
        {
            //建立点-弧段关系
            BuildPointArcs();

            //多边形自动构建算法
            foreach (var arc in Arcs.Where(arc => arc.TimesOfSearch == 0))
            {
                BuildPolygons(arc);
            }
        }

        /// <summary>
        /// 建立点-弧段关系
        /// </summary>
        public void BuildPointArcs()
        {
            foreach (var point in Points)
            {
                var pointArcs = new PointArcs { PointId = point.Id };
                foreach (var arc in Arcs)
                {

                    var startPoint = arc.Points[0];
                    var endPoint = arc.Points[arc.Points.Count - 1];

                    if (point.Id == startPoint.Id || point.Id == endPoint.Id)
                    {
                        pointArcs.Arcs.Add(arc);
                    }
                }
                if (pointArcs.Arcs.Count != 0)
                    PointArcses.Add(pointArcs);
            }
        }

        /// <summary>
        /// 从指定弧段开始构建多边形
        /// </summary>
        /// <param name="arc">弧段</param>
        public void BuildPolygons(Arc arc)
        {
            var polygons = new List<Polygon>();
            //正向（逆时针）构建第一个多边形
            var polygon = BuildPolygon(arc);
            polygon.Points = GetPolygonPoints(polygon);
            polygon.Area = Math.Abs(GetArea(polygon.Points));
            polygons.Add(polygon);

            //反向（顺时针）构建其它多边形
            for (var i = 0; i < Arcs.Count - 1; i++)
            {
                //从正向已经构建任意弧度开始构建
                if (Arcs[i].TimesOfSearch == 1)
                {
                    //改变当前弧段的构建方向
                    Arcs[i].Direction *= -1;
                    polygon = BuildPolygon(Arcs[i]);
                    if (polygon == null) return;
                    polygon.Points = GetPolygonPoints(polygon);
                    polygon.Area = Math.Abs(GetArea(polygon.Points));
                    polygons.Add(polygon);
                    //每次都从第一个弧段开始搜索
                    i = 0;
                }
            }

            //删除面积最大的一个多边形
            RemoveMaxPolygon(polygons);

            //给多边形增加编号
            foreach (var polygon1 in polygons)
            {
                polygon1.Id = Convert.ToChar(65 + PolygonIndex);
                Polygons.Add(polygon1);
                PolygonIndex++;
            }
        }

        /// <summary>
        /// 删除最大面积的多边形
        /// </summary>
        /// <param name="polygons">多边形</param>
        public void RemoveMaxPolygon(List<Polygon> polygons)
        {
            double max = 0.0f;
            var maxPolygon = new Polygon();
            foreach (var polygon in polygons.Where(polygon => max <= polygon.Area))
            {
                maxPolygon = polygon;
                max = polygon.Area;
            }
            polygons.Remove(maxPolygon);
        }

        /// <summary>
        /// 计算给定点集合的面积
        /// </summary>
        /// <param name="points">点集合</param>
        /// <returns>面积</returns>
        public double GetArea(List<Point> points)
        {
            double s = 0;
            for (int i = 0; i < points.Count - 1; i++)
                s += points[i].X * points[i + 1].Y - points[i + 1].X * points[i].Y;
            s += points[points.Count - 1].X * points[0].Y - points[0].X * points[points.Count - 1].Y;
            s /= 2.0;
            return s;
        }

        /// <summary>
        /// 获取多边形顶点集合
        /// </summary>
        /// <param name="polygon">多边形</param>
        /// <returns>点集合</returns>
        public List<Point> GetPolygonPoints(Polygon polygon)
        {
            var points = new List<Point>();
            foreach (var arcDirection in polygon.ArcDirections)
            {
                if (arcDirection.Direction == 1)
                {
                    points.AddRange(arcDirection.Arc.Points);
                }
                else
                {
                    points.AddRange(arcDirection.Arc.Points);
                    points.Reverse();
                }
            }
            return points;
        }

        /// <summary>
        /// 通过指定的弧段构建单个多边形
        /// </summary>
        public Polygon BuildPolygon(Arc arc)
        {
            //添加起始弧段
            var polygon = new Polygon();
            var arcDirection = new ArcDirection { Arc = arc, Direction = arc.Direction };
            polygon.ArcDirections.Add(arcDirection);

            //添加扩展弧段
            var nextArc = GetNextArc(arc);
            //扩展弧段不为起始弧段
            while (nextArc.Id != arc.Id)
            {
                nextArc.Direction = MinAngle.Direction;
                nextArc.TimesOfSearch += 1;

                var arcDir = new ArcDirection { Arc = nextArc, Direction = nextArc.Direction };
                polygon.ArcDirections.Add(arcDir);

                nextArc = GetNextArc(nextArc);
            }
            arc.TimesOfSearch += 1;
            return polygon;
        }

        /// <summary>
        /// 根据指定弧段获取下一个弧段
        /// </summary>
        /// <param name="arc">弧段</param>
        /// <returns>下一弧段</returns>
        public Arc GetNextArc(Arc arc)
        {
            var arcAngles = new List<ArcAngle>();
            var radian = 0.0d;
            if (arc.Direction == 1)
            {
                var start = arc.Points[arc.Points.Count - 2];
                var inflection = arc.Points[arc.Points.Count - 1];

                var pointArcs = GetPointArcsByPoint(inflection);
                foreach (var arc1 in pointArcs.Arcs)
                {
                    if (arc1.Id != arc.Id && arc1.TimesOfSearch < 2)
                    {
                        var arcAngle = new ArcAngle();
                        var sta = arc1.Points[0];
                        var end = arc1.Points[arc1.Points.Count - 1];

                        //确认扩展段
                        if (sta.Id == inflection.Id) //准扩展弧段以第一记录开始,方向与当前弧段相反
                        {
                            sta = arc1.Points[1];
                            radian = GetAngleOfTwoArcs(start, inflection, sta);
                            arcAngle.Direction = 1;
                        }
                        else if (end.Id == inflection.Id) //准扩展弧段以最后记录开始，方向与当前弧段一样
                        {
                            end = arc1.Points[arc1.Points.Count - 2];
                            radian = GetAngleOfTwoArcs(start, inflection, end);
                            arcAngle.Direction = -1;
                        }
                        arcAngle.Angle = radian;
                        arcAngle.Arc = arc1;

                        arcAngles.Add(arcAngle);
                    }
                }
            }
            else if (arc.Direction == -1)
            {
                var start = arc.Points[1];
                var inflection = arc.Points[0];

                var pointArcs = GetPointArcsByPoint(inflection);
                foreach (var arc1 in pointArcs.Arcs)
                {
                    if (arc1.Id != arc.Id && arc1.TimesOfSearch < 2)
                    {
                        var arcAngle = new ArcAngle();
                        var sta = arc1.Points[0];
                        var end = arc1.Points[arc1.Points.Count - 1];

                        if (sta.Id == inflection.Id)
                        {
                            sta = arc1.Points[1];
                            radian = GetAngleOfTwoArcs(start, inflection, sta);
                            arcAngle.Direction = 1;
                        }
                        else if (end.Id == inflection.Id)
                        {
                            end = arc1.Points[arc1.Points.Count - 2];
                            radian = GetAngleOfTwoArcs(start, inflection, end);
                            arcAngle.Direction = -1;
                        }
                        arcAngle.Angle = radian;
                        arcAngle.Arc = arc1;

                        arcAngles.Add(arcAngle);
                    }
                }
            }
            else
            {
                Console.WriteLine("弧段的方向错误!");
            }

            //没有任何夹角时，返回弧段本身
            if (arcAngles.Count == 0) return arc;

            //返回最小角度的弧段
            foreach (var arcAngle in arcAngles.Where(arcAngle => Math.Abs(arcAngle.Angle - arcAngles.Min(a => a.Angle)) < 0.0000001))
            {
                MinAngle = arcAngle;
            }
            return MinAngle.Arc;
        }

        /// <summary>
        /// 计算两条直线逆时针方向的夹角
        /// </summary>
        /// <param name="start">起始点</param>
        /// <param name="inflection">拐点</param>
        /// <param name="end">终止点</param>
        /// <returns>弧度</returns>
        public double GetAngleOfTwoArcs(Point start, Point inflection, Point end)
        {
            double aa = (start.X - inflection.X) * (start.X - inflection.X) +
                        (start.Y - inflection.Y) * (start.Y - inflection.Y);
            double bb = (inflection.X - end.X) * (inflection.X - end.X) +
                        (inflection.Y - end.Y) * (inflection.Y - end.Y);
            double cc = (start.X - end.X) * (start.X - end.X) +
                        (start.Y - end.Y) * (start.Y - end.Y);

            double cos = (aa + bb - cc) / (2 * Math.Sqrt(aa) * Math.Sqrt(bb));
            double angle = Math.Acos(cos);

            if (Math.Abs(start.Y - inflection.Y) >= 0.000001)
            {
                double coeff1 = (inflection.X - start.X) / (start.Y - inflection.Y);
                double coeff2 = (start.X * inflection.Y - inflection.X * start.Y) / (start.Y - inflection.Y);
                if (start.Y > inflection.Y)
                {
                    if (end.X + coeff1 * end.Y + coeff2 <= 0)
                        return angle;
                    return (Math.PI * 2 - angle);
                }
                if (start.Y < inflection.Y)
                {
                    if (end.X + coeff1 * end.Y + coeff2 <= 0)
                        return (Math.PI * 2 - angle);
                    return angle;
                }
            }
            else
            {
                if (inflection.X > start.X)
                {
                    if (end.Y < start.Y) return angle;
                    return (Math.PI * 2 - angle);
                }
                if (end.Y < start.Y) return (Math.PI * 2 - angle);
                return angle;
            }
            return 10.0f;
        }

        public PointArcs GetPointArcsByPoint(Point point)
        {
            foreach (var pointarcs in PointArcses.Where(pointarcs => pointarcs.PointId == point.Id))
            {
                return pointarcs;
            }
            return new PointArcs();
        }

    }
}
