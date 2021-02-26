using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace GIS_第五章_3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "所有文件（*.txt）|*.txt";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.textBox1.Text = openFileDialog.FileName;
                //this.textBox2.Text = Path.ChangeExtension(this.textBox1.Text, "_out.txt");
                this.textBox2.Text = Path.GetDirectoryName(textBox1.Text) + "/" + Path.GetFileNameWithoutExtension(textBox1.Text) + "_out.txt";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog openFileDialog = new SaveFileDialog();
            openFileDialog.Filter = "所有文件（*.txt）|*.txt";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;
            openFileDialog.FileName = Path.GetFileNameWithoutExtension(textBox1.Text) + "_out.txt";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.textBox2.Text = openFileDialog.FileName;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //读取输入的点弧关系文件
            if (textBox2.Text.Trim() == String.Empty || textBox1.Text.Trim() == String.Empty)
            {
                MessageBox.Show("请选择文件");
                return;
            }
            string[] pointslines = File.ReadAllLines(textBox1.Text);
            string[] arclines = File.ReadAllLines(textBox3.Text);
            var points = new List<Point>();
            var arcs = new List<Arc>();
            for (int i = 0; i < pointslines.Length; i++)
            {
                string[] line = pointslines[i].Split(' ');
                var p = new Point { Id = int.Parse(line[0]), X = float.Parse(line[1]), Y = float.Parse(line[2]) };
                points.Add(p);
            }
            for(int i = 1; i < arclines.Length; i++)
            {
                //var arcss = new List<Arc>();
                
                string[] line = arclines[i].Split(' ');
                var arc_p = new List<Point>();
                for (int j=1;j< line.Length;j++)
                {
                   
                    for (int k = 0;k< points.Count;k++)
                    {
                        if (int.Parse(line[j])==points[k].Id)
                        {
                            var p = new Point { Id = points[k].Id, X = points[k].X, Y = points[k].Y };
                            arc_p.Add(p);
                        }
                    }

                }
                var a = new Arc() { Id = line[0], Points = arc_p, Direction = 1 };
                arcs.Add(a);
            }
            var createPolygons = new CreatePolygons(points, arcs);
            //自动生成多边形，并记录有效信息
            createPolygons.MainCreatePolygons();
            //根据多边形中的信息建立拓扑关系
            var topology = new Topology(points);
            //建立弧段-点关系
            topology.BuildArcPoints(arcs);
            //建立多边形-弧段关系
            topology.BuildPolygonsArcs(createPolygons.Polygons);
            //建立弧段-多边形关系
            topology.BuildArcsPolygon(arcs, createPolygons.Polygons);
            //输出拓扑关系文件
            topology.PrintTopology(this.textBox2.Text);
            //Console.ReadLine();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "所有文件（*.txt）|*.txt";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.textBox3.Text = openFileDialog.FileName;
                //this.textBox2.Text = Path.ChangeExtension(this.textBox1.Text, "_out.txt");
                //this.textBox2.Text = Path.GetDirectoryName(textBox1.Text) + "/" + Path.GetFileNameWithoutExtension(textBox1.Text) + "_out.txt";
            }
        }
    }
}
