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
using System.Drawing.Imaging;
namespace Mypaint_
{
    public partial class Form1 : Form
    {
        private int mouseStartX = 0;
        private int mouseStartY = 0;
        private int mouseCurrentX = 0;
        private int mouseCurrentY = 0;
        private int recSartPointX = 0;
        private int recSartPointY = 0;
        private int recSizeY = 0;
        private int recSizeX = 0;
        private bool mouseDown = false;
        private Bitmap bm;
        private int shapeSelected = 0;
        private Color setPaintColor;
        //private SetPanel canvas = new SetPanel();
        
        //private Color setPaintColorFill;

        public Form1()
        {
            InitializeComponent();
            bm = new Bitmap(canvas.Width, canvas.Height);
            setPaintColor = Color.Black;
            
            
            //setPaintColorFill = Color.Violet;
        }


        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            shapeSelected = 2;
            canvas.Cursor = Cursors.Cross;
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                setPaintColor = colorDialog1.Color;
                toolStripButton7.BackColor = colorDialog1.Color;
            }    
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to clear drawing and start new?", "Clear", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                bm = new Bitmap(canvas.Width, canvas.Height);
                canvas.BackgroundImage = bm;
                Graphics setPaint = Graphics.FromImage(bm);
                setPaint.Clear(canvas.BackColor);
                setPaint.Dispose();
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Bitmap bm = new Bitmap(canvas.Width+270, canvas.Height+94);
            Graphics setPaint = Graphics.FromImage(bm);
            Rectangle rect = canvas.RectangleToScreen(canvas.ClientRectangle);
            rect = new Rectangle(rect.Location.X + 54, rect.Location.Y + 54, rect.Size.Width,rect.Size.Height);
            Size size = new Size(canvas.Size.Width + 270, canvas.Size.Height + 94);
            setPaint.CopyFromScreen(rect.Location, Point.Empty, size);
            //canvas.Dispose();

            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Png files| *.png| jpeg files| *jpg| bitmaps| *bmp";

            if (save.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    if (File.Exists(save.FileName))
                    {
                        File.Delete(save.FileName);
                    }
                    if (save.FileName.Contains(".jpg"))
                    {
                        bm.Save(save.FileName, ImageFormat.Jpeg);
                    }
                    else if (save.FileName.Contains(".png"))
                    {
                        bm.Save(save.FileName, ImageFormat.Png);
                    }
                    else if (save.FileName.Contains(".bmp"))
                    {
                        bm.Save(save.FileName, ImageFormat.Bmp);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("File save error : " + ex.Message);
                }
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Png files| *.png| jpeg files| *jpg| bitmaps| *bmp";
            if (open.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    Bitmap oldbm = bm;
                    bm = new Bitmap(open.FileName);
                    oldbm.Dispose();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading image " + ex.Message);
                }

            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            shapeSelected = 3;
            canvas.Cursor = Cursors.Cross;
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            shapeSelected = 1;
            canvas.Cursor = Cursors.Default;
        }

        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics setPaint = e.Graphics;
            if (mouseDown == true)
            {
                /*Sets the thickness of the line or boarder of the shapes.*/
                Pen size = new Pen(setPaintColor, 1);
                if (shapeSelected == 1)
                {
                    /*Draws line*/
                    setPaint.DrawLine(size, new Point(mouseStartX, mouseStartY), new Point(mouseCurrentX + mouseStartX, mouseCurrentY + mouseStartY));
                }
                else if (shapeSelected == 2)
                {
                    //Draws ellipse boarder.
                    setPaint.DrawEllipse(size, mouseStartX, mouseStartY, mouseCurrentX, mouseCurrentY);
                }
                else if (shapeSelected == 3)
                {
                    //Draws rectangle boarder.
                    setPaint.DrawRectangle(size, recSartPointX, recSartPointY, recSizeX, recSizeY);
                }
               
            }
            //Paints to bitmap
            setPaint.DrawImage(bm, new Point(0, 0));
        }

        private void canvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //This is the size of the line or the boarder of the shape.
                Pen size = new Pen(setPaintColor, 1);

                mouseDown = false;
                Graphics setPaint = Graphics.FromImage(bm);

                if (shapeSelected == 1)
                {
                    //Draws the line
                    setPaint.DrawLine(size, new Point(mouseStartX, mouseStartY), new Point(mouseCurrentX + mouseStartX, mouseCurrentY + mouseStartY));
                }
                else if (shapeSelected == 2)
                {
                    //draws the ellipse with the boarder.
                    setPaint.DrawEllipse(size, mouseStartX, mouseStartY, mouseCurrentX, mouseCurrentY);
                    //setPaint.FillEllipse(new SolidBrush(setPaintColorFill), mouseStartX, mouseStartY, mouseCurrentX, mouseCurrentY);
                }
                else if (shapeSelected == 3)
                {
                    //Draws the rectangle with the boarder.
                    setPaint.DrawRectangle(size, recSartPointX, recSartPointY, recSizeX, recSizeY);
                    //setPaint.FillRectangle(new SolidBrush(setPaintColorFill), recSartPointX, recSartPointY, recSizeX, recSizeY);
                }
            }
        }
        private Point start;
        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown == true)
            {
                Graphics setPaint = Graphics.FromImage(bm);
                
                mouseCurrentX = e.X - mouseStartX;
                mouseCurrentY = e.Y - mouseStartY;
                
                if (shapeSelected==4)
                {
                    Pen size = new Pen(setPaintColor, 1);
                    setPaint.DrawLine(size, start, e.Location);
                    setPaint.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    start = e.Location;
                }    
                canvas.Invalidate();
                
            }
            else
            {
                canvas.Invalidate();
            }
            //Calculate and determine where the rectangle should be drawn.
            //The x-value of our rectangle should be the minimum between the start x-value and the current x-position
            recSartPointX = Math.Min(mouseStartX, e.X);
            //The y-value of our rectangle should also be the minimum between the start y-value and current y-value
            recSartPointY = Math.Min(mouseStartY, e.Y);
            //The width (recSizeX) of our rectangle should be the maximum between the start x-position and current x-position minus
            //the minimum of start x-position and current x-position
            recSizeX = Math.Max(mouseStartX, e.X) - Math.Min(mouseStartX, e.X);
            //For the hight(recSizeY) value, it's basically the same thing as above, but now with the y-values:
            recSizeY = Math.Max(mouseStartY, e.Y) - Math.Min(mouseStartY, e.Y);
        }

        private void canvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseDown = true;
                mouseStartX = e.X;
                mouseStartY = e.Y;
                start = e.Location;
            }
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            shapeSelected = 4;
            canvas.Cursor = Cursors.Default;
        }
       

    }
}
