using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NeuralNetPaintPicture
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private string path = @"";

        private PictureNeuralFilter pictureNeuralFilter;
        private Bitmap picture;
        public static Label label;
        private string newfileName = "New file";

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text += PointsArranger.Radius;
            textBox2.Text += PointsArranger.MinDistance;
            textBox3.Text += PointsArranger.PointsCount;
            textBox4.Text += PointsArranger.seed;
            textBox6.Text += PicturePainter.valueForReplaceZero;
            textBox7.Text += "New file";
            checkBox1.Checked = true;
            checkBox2.Checked = false;
            checkBox3.Checked = true;

            pictureNeuralFilter = new PictureNeuralFilter();

            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = pictureNeuralFilter.CreatePicture(picture);
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if(textBox1.Text != "")
                PointsArranger.Radius = Convert.ToDouble(textBox1.Text);
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if(textBox2.Text != "")
                PointsArranger.MinDistance = Convert.ToDouble(textBox2.Text);
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if(textBox3.Text != "")
                PointsArranger.PointsCount = Convert.ToInt32(textBox3.Text);
        }
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if(textBox4.Text != "")
                PointsArranger.seed = Convert.ToInt32(textBox4.Text);
        }
        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if(textBox6.Text != "")
                PicturePainter.valueForReplaceZero = Convert.ToInt32(textBox6.Text);
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            PointsArranger.makeRandomPointsWithControl = checkBox1.Checked;
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            PicturePainter.useSkips = checkBox2.Checked;

        }
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            PicturePainter.shouldReplaceZero = checkBox3.Checked;

        }
        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            if (textBox7.Text == "")
                newfileName = "New Picture";
            newfileName = textBox7.Text;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            pictureBox2.Image.Save(newfileName + ".jpg");
        }
        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                path = dialog.FileName;
                button1.Enabled = true;
                picture = new Bitmap(path);
                pictureBox1.Image = picture;
                label6.Text = dialog.FileName;
            }
        }
    }
}
