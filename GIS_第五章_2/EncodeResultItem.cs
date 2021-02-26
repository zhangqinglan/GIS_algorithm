using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIS_第五章_2
{
    class EncodeResultItem
    {
        private int mMorton;
        private string mPixelColor;
        private int mBlockSize;
        private int mX;
        private int mY;

        public int Morton
        {
            get { return mMorton; }
            set { mMorton = value; }
        }
        public string PixelColor
        {
            get { return mPixelColor; }
            set { mPixelColor = value; }
        }
        public int BlockSize
        {
            get { return mBlockSize; }
            set { mBlockSize = value; }
        }
        public int X
        {
            get { return mX; }
            set { mX = value; }
        }
        public int Y
        {
            get { return mY; }
            set { mY = value; }
        }
    }
}
