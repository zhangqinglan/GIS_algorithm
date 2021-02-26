using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GIS_第五章_2
{
    class Quad_TreeCode
    {
        Bitmap mOriginalBitmap;

        public Quad_TreeCode(Bitmap pBitmap)
        {
            mOriginalBitmap = pBitmap;


        }
        private Bitmap SplitQuarterImage(Bitmap pBitmap, string pQuarterString)
        {

            int N = pBitmap.Width;
            Bitmap bitmap = new Bitmap(N / 2, N / 2);

            if (N < 4)
            {
                return null;
            }
            if (pQuarterString == "NW")
            {

                RectangleF NWRect = new RectangleF(0, 0, N / 2, N / 2);
                bitmap = pBitmap.Clone(NWRect, System.Drawing.Imaging.PixelFormat.DontCare);
            }
            if (pQuarterString == "NE")
            {

                RectangleF NERect = new RectangleF(N / 2, 0, N / 2, N / 2);
                bitmap = pBitmap.Clone(NERect, System.Drawing.Imaging.PixelFormat.DontCare);
            }
            if (pQuarterString == "SW")
            {

                RectangleF SWRect = new RectangleF(0, N / 2, N / 2, N / 2);

                bitmap = pBitmap.Clone(SWRect, System.Drawing.Imaging.PixelFormat.DontCare);
            }
            if (pQuarterString == "SE")
            {

                RectangleF SERect = new RectangleF(N / 2, N / 2, N / 2, N / 2);


                bitmap = pBitmap.Clone(SERect, System.Drawing.Imaging.PixelFormat.DontCare);
            }
            return bitmap;



        }

        public string GetEncodeString()
        {

            string encodeString = string.Empty;
            IEnumerable<EncodeResultItem> encodeResults = Encode();

            foreach (EncodeResultItem item in encodeResults)
            {
                encodeString +=
                    item.Morton.ToString()
                    + " "
                    + item.PixelColor
                    + Environment.NewLine;
            }

            return encodeString;
        }
        public IEnumerable<EncodeResultItem> Encode()
        {
            QNode rootNode;
            QuadTree tree;

            rootNode = new QNode();
            rootNode.Data = mOriginalBitmap;
            rootNode.ParentNode = null;
            rootNode.Deep = 0;
            rootNode.Location.X = 0;
            rootNode.Location.Y = 0;

            tree = new QuadTree();
            tree.Nodes.Add(rootNode);

            tree = BuildTree(rootNode, tree);


            List<QNode> nodes = new List<QNode>();

            for (int i = 0; i < tree.Nodes.Count; i++)
            {
                if (tree.Nodes[i].NENode == null)
                {
                    nodes.Add(tree.Nodes[i]);
                }

            }

            List<EncodeResultItem> encodeRestults = new List<EncodeResultItem>();

            foreach (QNode node in nodes)
            {
                EncodeResultItem rsltItem = new EncodeResultItem();

                rsltItem.Morton = GetMorton(node.Location.X, node.Location.Y);
                rsltItem.PixelColor = ((Bitmap)node.Data).GetPixel(0, 0).ToArgb().ToString(); ;


                encodeRestults.Add(rsltItem);


            }

            IEnumerable<EncodeResultItem> resultsOrderbyMorton
                = encodeRestults.OrderBy(rsltItem6 => rsltItem6.Morton);



            return resultsOrderbyMorton;

        }
        private QuadTree BuildTree(QNode pNode, QuadTree pTree)
        {
            //pTree.Nodes.Add(pNode);
            Bitmap pBitmap = (Bitmap)pNode.Data;

            if (pBitmap.Width >= 4 && HasDiffrentColor(pBitmap))
            {


                Bitmap NWBitmap = SplitQuarterImage(pBitmap, "NW");
                QNode NWNode = new QNode(pNode, null, null, null, null, NWBitmap);
                NWNode.Deep = pNode.Deep + 1;
                NWNode.Location.X = pNode.Location.X;
                NWNode.Location.Y = pNode.Location.Y;

                Bitmap NEBitmap = SplitQuarterImage(pBitmap, "NE");
                QNode NENode = new QNode(pNode, null, null, null, null, NEBitmap);
                NENode.Deep = pNode.Deep + 1;
                NENode.Location.X = pNode.Location.X + ((Bitmap)pNode.Data).Width / 2;
                NENode.Location.Y = pNode.Location.Y;

                Bitmap SWBitmap = SplitQuarterImage(pBitmap, "SW");
                QNode SWNode = new QNode(pNode, null, null, null, null, SWBitmap);
                SWNode.Deep = pNode.Deep + 1;
                SWNode.Location.X = pNode.Location.X;
                SWNode.Location.Y = pNode.Location.Y + ((Bitmap)pNode.Data).Height / 2;

                Bitmap SEBitmap = SplitQuarterImage(pBitmap, "SE");
                QNode SENode = new QNode(pNode, null, null, null, null, SEBitmap);
                SENode.Deep = pNode.Deep + 1;
                SENode.Location.X = pNode.Location.X + ((Bitmap)pNode.Data).Width / 2;
                SENode.Location.Y = pNode.Location.Y + ((Bitmap)pNode.Data).Height / 2;


                pNode.NWNode = NWNode;
                pNode.NENode = NENode;
                pNode.SENode = SENode;
                pNode.SWNode = SWNode;

                pTree.Nodes.Add(NWNode);
                pTree.Nodes.Add(NENode);
                pTree.Nodes.Add(SWNode);
                pTree.Nodes.Add(SENode);

                BuildTree(NWNode, pTree);
                BuildTree(NENode, pTree);
                BuildTree(SWNode, pTree);
                BuildTree(SENode, pTree);

            }

            return pTree;

        }
        public int GetMorton(int x, int y)
        {
            int binaryLength = 16;
            string zeros = "0000000000000000";

            string BinaryMortonCode = string.Empty;
            //行列号与屏幕坐标相反,故赋值时取反--------------
            string binaryRowNo = Convert.ToString(y, 2);
            string binaryColumnNo = Convert.ToString(x, 2);
            //---------------------------------------------
            if (binaryRowNo.Length < binaryLength)
            {
                binaryRowNo =
                    zeros.Substring(0, binaryLength - binaryRowNo.Length) + binaryRowNo;
            }
            if (binaryColumnNo.Length < binaryLength)
            {
                binaryColumnNo =
                     zeros.Substring(0, binaryLength - binaryColumnNo.Length) + binaryColumnNo;
            }

            for (int i = 0; i < binaryLength; i++)
            {
                BinaryMortonCode = BinaryMortonCode +
                    binaryRowNo.Substring(i, 1) + binaryColumnNo.Substring(i, 1);
            }


            int MoronCode = Convert.ToInt32(BinaryMortonCode, 2);
            return MoronCode;
        }
        private bool HasDiffrentColor(Bitmap pBitmap)
        {
            Color pixelColor1 = pBitmap.GetPixel(0, 0);
            Color pixelColor2;

            for (int i = 0; i < pBitmap.Width; i++)
            {
                for (int j = 0; j < pBitmap.Height; j++)
                {
                    pixelColor2 = pBitmap.GetPixel(i, j);

                    if (pixelColor1 != pixelColor2)
                    {
                        return true;

                    }
                }
            }
            return false;
        }
    }
}
