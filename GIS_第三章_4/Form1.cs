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


namespace GIS_第三章_4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        double Pi = 3.1415926535898;
        List<int> H = new List<int>();
        List<double> B = new List<double>();//存储转换后的B坐标
        List<double> L = new List<double>();//存储转换后的L坐标
        List<double> BMercator = new List<double>();
        List<double> LMercator = new List<double>();
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
            if (textBox3.Text != "" && textBox4.Text != "" && textBox5.Text != "")
            {
                double B0 = Convert.ToDouble(textBox3.Text) * Math.PI / 180;
                double L0 = Convert.ToDouble(textBox4.Text) * Math.PI / 180;
                double B1 = Convert.ToDouble(textBox5.Text) * Math.PI / 180;
                inputdata();
                Mercator2lonLat();
                outputdata();
                MessageBox.Show("墨卡托投影反解成功");
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
                BMercator.Add(Convert.ToDouble(Q[0]));
                LMercator.Add(Convert.ToDouble(Q[1]));
            }
            str.Close();
        }
        //墨卡托转经纬度
        public void Mercator2lonLat()
        {
            for (int i = 0; i < BMercator.Count; i++)
            {
                double x = BMercator[i]/ 20037508.34 * 180;
                double y = LMercator[i] / 20037508.34 * 180;
                y = 180 / Pi * (2 * System.Math.Atan(System.Math.Exp(y * Pi / 180)) - Pi / 2);
                B.Add(x);
                L.Add(y);
            }
        }
        //输出投影后的数据
        public void outputdata()
        {
            List<string> lines = new List<string>();
            for (int i = 0; i < B.Count; i++)
            {
                lines.Add(B[i] + "," + L[i]);
            }
            File.WriteAllLines(this.textBox2.Text, lines);
        }
    }
}
