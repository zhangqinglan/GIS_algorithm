using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GIS_第五章_2
{
    public class QNode
    {
        public QNode ParentNode;
        public QNode NWNode;
        public QNode NENode;
        public QNode SWNode;
        public QNode SENode;
        public object Data;
        public int Deep;
        public Point Location;
        public string PixelColor;
        public int Morton;

        public QNode()
            : this(null, null, null, null, null, null)
        {

        }

        public QNode(QNode pntNode, QNode nw, QNode ne, QNode sw, QNode se, object e)
        {
            this.ParentNode = pntNode;
            this.NWNode = nw;
            this.NENode = ne;
            this.SWNode = sw;
            this.SENode = se;
            this.Data = e;
        }

        public QNode(Point pLocation, int pDeep, string pPixelColor, int pMorton)
        {
            this.Location = pLocation;
            this.Deep = pDeep;
            this.PixelColor = pPixelColor;
            this.Morton = pMorton;
        }
    }
}
