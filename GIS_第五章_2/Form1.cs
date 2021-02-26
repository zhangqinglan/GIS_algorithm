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

namespace GIS_第五章_2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Bitmap image;
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "所有文件（*.tif）|*.tif|所有文件（*.jpg）|*.jpg";
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
            //读取栅格数据
            image = new Bitmap(this.textBox1.Text);
            try
            {
                Quad_TreeCode q = new Quad_TreeCode(image);
                textBox3.Text = q.GetEncodeString();
                File.WriteAllText(this.textBox2.Text, q.GetEncodeString());

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            //
        }
    }
}
