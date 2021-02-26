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
using System.Collections;

namespace GIS_第五章_1
{
    public partial class Form1 : Form
    {
        int num;
        ArrayList myar = new ArrayList();//存入原始数据  
        ArrayList yssj = new ArrayList();//原始数据的坐标
        Dictionary<int, float> d = new Dictionary<int, float>();
        Dictionary<float, float> m = new Dictionary<float, float>();
        ArrayList newar = new ArrayList(); //存入抽稀后的数据的坐标 

        public class canshu//记录直线参数的类  
        {
            public float k;
            public float b;
        }

        public class zuobiao//坐标数据类  
        {
            public float x;
            public float y;

        }
        public canshu xielv(zuobiao shou, zuobiao wei)//求斜率  
        {
            float k, b;
            canshu newcs = new canshu();
            k = (wei.y - shou.y) / (wei.x - shou.x);
            b = shou.y - k * shou.x;
            newcs.k = k;
            newcs.b = b;
            return newcs;
        }

        public float distance(zuobiao dot, canshu cs)//求点到直线距离  
        {
            float dis = (float)(Math.Abs(cs.k * dot.x - dot.y + cs.b)) / (float)Math.Sqrt(cs.k * cs.k + 1);//点（x0，y0）到直线Ax+By+c=0的距离d=|Ax0+By0+c|/(A2+B2)1/2
            return dis;
        }
        public void Douglas(int number1, int number2)
        {
            int max = 0;//定义拥有最大距离值的点的编号  
            canshu myc = new canshu();
            myc = xielv((zuobiao)yssj[number1], (zuobiao)yssj[number2 - 1]);
            max = 0;
            float maxx = distance((zuobiao)yssj[number1 + 1], myc);//假设第二个点为最大距离点  

            float yuzhi = Convert.ToSingle(textBox3.Text);//设阈值 

            for (int i = number1 + 1; i < number2 - 1; i++)//从第二个点遍历到最后一个点前方的点  
            {
                if (distance((zuobiao)yssj[i], myc) > yuzhi && distance((zuobiao)yssj[i], myc) >= maxx)//找出拥有最大距离的点  
                {
                    max = i;
                    maxx = distance((zuobiao)yssj[i], myc);
                }
            }
            if (max == 0)//若不存在最大距离点，则只将首尾点存入arraylist，结束这一次的道格拉斯抽稀  
            {
                if (newar.Contains((zuobiao)yssj[number2 - 1]))
                {
                    return;
                }
                else
                {
                    newar.Add((zuobiao)yssj[number2 - 1]);
                    return;
                }
            }
            else if (number1 + 1 == max && number2 - 2 != max)//如果第二个点是最大距离点，则以下一个点和尾点作为参数进行道格拉斯抽稀释  
            {
                newar.Add((zuobiao)yssj[max]);
                Douglas(max, number2);
            }
            else if (number2 - 2 == max && number1 + 1 != max)//<span style="font-family: Arial;">如果倒数第二个点是最大距离点，则以首点和倒数第三点作为参数进行道格拉斯抽稀  
            {
                newar.Add((zuobiao)yssj[max]);
                Douglas(number1, max + 1);
            }
            else if (number1 + 1 == max && number2 - 2 == max)//如果首点尾点夹住最大距离点，则将最大距离点和尾点存入arraylist  
            {
                newar.Add((zuobiao)yssj[max]);
                return;
            }
            else
            {
                newar.Add((zuobiao)yssj[max]);
                Douglas(number1, max + 1);
                Douglas(max, number2);
            }
        }
        //newar里边的坐标按横坐标进行排序
        public class yssjCompare : IComparer
        {
            public int Compare(object pA, object pB)
            {
                zuobiao p1 = (zuobiao)pA;
                zuobiao p2 = (zuobiao)pB;
                return p1.x.CompareTo(p2.x);
            }
        }
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
                this.textBox2.Text = Path.GetDirectoryName(textBox1.Text) +"/"+Path.GetFileNameWithoutExtension(textBox1.Text) + "_out.txt";
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
            if (textBox2.Text.Trim() == String.Empty|| textBox1.Text.Trim() == String.Empty)
            {
                MessageBox.Show("请选择文件");
                return;
            }
            if (textBox3.Text.Trim() == String.Empty)
            {
                MessageBox.Show("阈值不能为空，请输入阈值");
                return;
            }
            d.Clear();
            m.Clear();
            yssj.Clear();
            newar.Clear();
            myar.Clear();
            //处理读入的原始数据
            readFile();
            num = yssj.Count;
            newar.Clear();
            newar.Add((zuobiao)yssj[0]);
            try {
                Douglas(0, num);
                if (newar.Contains((zuobiao)yssj[num - 1]))
                { }
                else
                {
                    newar.Add((zuobiao)yssj[num - 1]);
                }
                List<string> newlines = new List<string>();
                for (int i = 0; i < newar.Count; i++)
                {
                    newlines.Add(((zuobiao)newar[i]).x.ToString() + "," + ((zuobiao)newar[i]).y.ToString());
                }

                //将抽稀后的数据写入文本中
                File.WriteAllLines(textBox2.Text, newlines);
                MessageBox.Show("压缩成功", "提示信息");
            } catch (Exception ex)
            {
                MessageBox.Show("压缩失败，" + ex.Message, "提示信息");
            }
        }

        public void readFile()
        {
            //读取文件中全部行
            string[] lines = File.ReadAllLines(textBox1.Text);
            //将每一行按逗号进行分割，并形成坐标
            for (int i=0;i<lines.Length;i++)
            {
                if (!string.IsNullOrEmpty(lines[i]))
                {
                    string[] line = lines[i].Split(',');
                    zuobiao temp = new zuobiao();
                    temp.x = float.Parse(line[0]);
                    temp.y = float.Parse(line[1]);
                    yssj.Add(temp);
                }
            }

        }
    }
}
