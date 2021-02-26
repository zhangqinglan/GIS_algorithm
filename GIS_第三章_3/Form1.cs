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

namespace GIS_第三章_3
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
        List<double> BMercator = new List<double>();//存储转换后的B坐标
        List<double> LMercator = new List<double>();//存储转换后的L坐标

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

        private void btnMocato_Click(object sender, EventArgs e)
        {
            if (textBox3.Text != "" && textBox4.Text != "" && textBox5.Text != "")
            {
                double B0 = Convert.ToDouble(textBox3.Text) * Math.PI / 180;
                double L0 = Convert.ToDouble(textBox4.Text) * Math.PI / 180;
                double B1 = Convert.ToDouble(textBox5.Text) * Math.PI / 180;
                inputdata();
                Mercator(B0, B1, e1, e2, L0);
                outputdata();
                MessageBox.Show("墨卡托投影成功");
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
        public void Mercator(double B0, double B1, double e1, double e2, double L0)
        {
            double K = Math.Pow(a, 2) / b / Math.Sqrt(1 + Math.Pow(e2, 2) * Math.Pow(Math.Cos(B0), 2))
                    * Math.Cos(B0);
            List<double> exchange = new List<double>();
            List<double> m = new List<double>();
            List<double> n = new List<double>();

            for (int i = 0; i < B.Count; i++)
            {
                exchange.Add(B[i]);
                B[i] = L[i] * Math.PI / 180;
                L[i] = exchange[i] * Math.PI / 180;
                m.Add(Math.Tan(Math.PI / 4 + B[i] / 2));
                n.Add((1 - e1 * Math.Sin(B[i])) / (1 + e1 * Math.Sin(B[i])));
                BMercator.Add(K * Math.Log(m[i] + Math.Pow(n[i], (e1 / 2))) - 2800000);
                LMercator.Add(K * (L[i] - L0) - 50000);
            }
        }
        //输出投影后的数据
        public void outputdata()
        {
            List<string> lines = new List<string>();
            for (int i = 0; i < BMercator.Count; i++)
            {
                lines.Add(BMercator[i] + "," + LMercator[i]);
            }
            File.WriteAllLines(this.textBox2.Text, lines);
        }
    }
}
