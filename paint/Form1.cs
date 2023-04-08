using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace paint
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            radioButton1.Checked = pen;
            radioButton2.Checked = rectangle;
            radioButton3.Checked = ellipse;
            panel1.BackColor = myPen.Color;
        }

        bool pen = true;
        bool rectangle = false;
        bool ellipse = false;

        static SolidBrush brush = new SolidBrush(Color.Black);
        Pen myPen = new Pen(brush,1);

        Point startPoint;
        bool isMouseDown = false;

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true;
            startPoint = e.Location;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                if(startPoint != null)
                {
                    if (pen)
                    {
                        if (pictureBox1.Image == null)
                        {
                            Bitmap tempBmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                            pictureBox1.Image = tempBmp;    

                        }
                        using (Graphics g = Graphics.FromImage(pictureBox1.Image))
                        {
                           if(e.Button == MouseButtons.Left)
                           {
                                myPen.Color = panel1.BackColor;
                           }

                           else if (e.Button == MouseButtons.Right)
                           {
                                myPen.Color = Color.White;
                           }
                            g.DrawLine(myPen, startPoint, e.Location);
                            pictureBox1.Invalidate();
                        }
                    }
                }
            }
            if (pen)
            {
                startPoint = e.Location;
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (rectangle || ellipse)
            {
                if (pictureBox1.Image == null)
                {
                    Bitmap tempBmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                    pictureBox1.Image = tempBmp;
                }
                using (Graphics g = Graphics.FromImage(pictureBox1.Image))
                {
                    if(rectangle)
                    {
                        if(e.Location.X > startPoint.X && e.Location.Y > startPoint.Y)
                        {
                            g.DrawRectangle(myPen, startPoint.X,startPoint.Y,e.Location.X - startPoint.X, e.Location.Y - startPoint.Y);
                        }
                    }
                    if (ellipse)
                    {
                        g.DrawEllipse(myPen, startPoint.X, startPoint.Y, e.Location.X - startPoint.X, e.Location.Y - startPoint.Y);
                    }
                }
                pictureBox1.Invalidate();
            }
            startPoint = Point.Empty;
            isMouseDown = false;    
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            myPen.Width = (float)numericUpDown1.Value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                myPen.Color = colorDialog1.Color;
                panel1.BackColor = colorDialog1.Color; 

            }

        }
        private void ChangeInstrument()
        {
            if(radioButton1.Checked)
            {
                pen = true;
                rectangle = false;
                ellipse = false;
            }
            if (radioButton2.Checked)
            {
                pen = false;
                rectangle = true;
                ellipse = false;
            }
            if(radioButton3.Checked)
            {
                pen = false;
                rectangle = false;
                ellipse = true;
            }
        }

        private void radioButtons_CheckedChanged(object sender, EventArgs e)
        {
            ChangeInstrument();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(pictureBox1.Image != null)
            {
                pictureBox1.Image = null;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = "MyImg";
            saveFileDialog1.DefaultExt = ".jpg";
            saveFileDialog1.Filter = "JPEG FILES(*.jpg)|*.jpg|All files(*.*)|*.*";
            saveFileDialog1.Title = "Saving your Img......";
            if(saveFileDialog1.ShowDialog() == DialogResult.OK )
            {
                Bitmap buffer = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                pictureBox1.DrawToBitmap(buffer, new Rectangle(0,0,pictureBox1.Width,pictureBox1.Height));
                buffer.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            openFileDialog1.DefaultExt = ".jpg";
            openFileDialog1.Filter = "JPEG FILES(*.jpg)|*.jpg|All files(*.*)|*.*";
            openFileDialog1.Title = "Open your Img......";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                Bitmap openimage = new Bitmap(openFileDialog1.FileName);
                if(pictureBox1.Image == null)
                {
                    pictureBox1.Image = bmp;
                }
                using (Graphics g = Graphics.FromImage(pictureBox1.Image))
                {

                 g.DrawImage(openimage, 0, 0 ,pictureBox1.Width, pictureBox1.Height);

                }  
            }
        }
    }
}