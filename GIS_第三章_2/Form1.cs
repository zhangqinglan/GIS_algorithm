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

namespace GIS_第三章_2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //54坐标系参数
        double a = 6378245;
        double b = 6356863.01877;
        double e1 = Math.Sqrt(0.0066943849995888);
        double e2 = Math.Sqrt(0.006739501819473);


        List<int> H = new List<int>();
        List<double> B = new List<double>();
        List<double> L = new List<double>();
        List<double> BLambert = new List<double>();//存储转换后的B坐标
        List<double> LLambert = new List<double>();//存储转换后的L坐标

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

        private void BtnLambert_Click(object sender, EventArgs e)
        {
            if (textBox3.Text != "" && textBox4.Text != "" && textBox5.Text != "" && textBox6.Text != "")
            {
                double B0 = Convert.ToDouble(textBox3.Text) * Math.PI / 180;
                double L0 = Convert.ToDouble(textBox4.Text) * Math.PI / 180;
                double B1 = Convert.ToDouble(textBox5.Text) * Math.PI / 180;
                double B2 = Convert.ToDouble(textBox6.Text) * Math.PI / 180;
                inputdata();
                Lambert(B0, B1, B2, e1, L0);
                outputdata();
                MessageBox.Show("兰伯特等角投影成功");
            }
            else
                MessageBox.Show("请输入数据");
        }
        //数据读入
        public void inputdata()
        {
            StreamReader str = new StreamReader(this.textBox1.Text);
            string x;
            while ((x = str.ReadLine()) != null)
            {
                string[] Q = x.Split(',');
                B.Add(Convert.ToDouble(Q[0]));
                L.Add(Convert.ToDouble(Q[1]));
            }
            str.Close();
        }
        public void Lambert(double B0, double B1, double B2, double e1, double L0)
        {
            List<double> m = new List<double>();
            List<double> t = new List<double>();
            List<double> r = new List<double>();
            List<double> q = new List<double>();
            List<double> c = new List<double>();

            double m0 = Math.Cos(B0) / Math.Sqrt(1 - e1 * e1 * Math.Sin(B0) * Math.Sin(B0));//m0常数
            double m1 = Math.Cos(B1) / Math.Sqrt(1 - e1 * e1 * Math.Sin(B1) * Math.Sin(B1));//mB1常数
            double m2 = Math.Cos(B2) / Math.Sqrt(1 - e1 * e1 * Math.Sin(B2) * Math.Sin(B2));//mB2常数
            double t0 = Math.Tan(Math.PI / 4 - B0 / 2) / Math.Pow((1 - e1 * Math.Sin(B0) / (1 + e1 * Math.Sin(B0))), (e1 / 2));//t0常数
            double t1 = Math.Tan(Math.PI / 4 - B1 / 2) / Math.Pow((1 - e1 * Math.Sin(B1) / (1 + e1 * Math.Sin(B1))), (e1 / 2));//tB1常数
            double t2 = Math.Tan(Math.PI / 4 - B2 / 2) / Math.Pow((1 - e1 * Math.Sin(B2) / (1 + e1 * Math.Sin(B2))), (e1 / 2));//tB2常数
            double n = Math.Log10(m1 / m2) / Math.Log10(t1 / t2);
            double F = m1 / (n * Math.Pow(t1, n));
            double r0 = a * F * Math.Pow(t0, n);
            for (int i = 0; i < B.Count; i++)
            {
                c.Add(B[i]);
                B[i] = L[i] * Math.PI / 180;
                L[i] = c[i] * Math.PI / 180;
                m.Add(Math.Cos(B[i]) / (Math.Sqrt(1 - e1 * e1 * Math.Sin(B[i]) * Math.Sin(B[i]))));
                t.Add(Math.Tan(Math.PI / 4 - B[i] / 2) / Math.Pow((1 - e1 * Math.Sin(B[i]) / (1 + e1 * Math.Sin(B[i]))), (e1 / 2)));
                r.Add(a * F * Math.Pow(t[i], n));
                q.Add(n * (L[i] - L0));
                BLambert.Add((r0 - r[i] * Math.Cos(q[i])));
                LLambert.Add((r[i] * Math.Sin(q[i])));
            }
        }
        //输出投影后的数据
        public void outputdata()
        {
            List<string> lines = new List<string>();
            for (int i=0;i< BLambert.Count;i++)
            {
                lines.Add(BLambert[i]+","+ LLambert[i]);
            }
            File.WriteAllLines(this.textBox2.Text, lines);
        }

    }
}
