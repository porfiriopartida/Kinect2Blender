using System;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using System.Windows;

namespace HelloKinect
{
    public partial class ThreadForm : Form
    {
        public ThreadForm()
        {
            InitializeComponent();
            pen = new System.Drawing.Pen(Brushes.Red, 2);
            panel1.Paint += new PaintEventHandler(panel1_Paint);
        }
        //declare the delegate that we'll use to launch our worker function in a separate thread
        public delegate void workerFunctionDelegate(int totalSeconds);

        //declare the delegate that we'll use to call the function that displays text in our text box
        public delegate void poplateTextBoxDelegate(string text);

        void populateTextBox(string text)
        {
            textBox1.Text = textBox1.Text + " " + text;
        }

        //this function simulates "work" by simply counting from 1 to totalSeconds
        void workerFunction(int totalSeconds)
        {
            for (int count = 1; count <= totalSeconds; count++)
            {
                //we use this.Invoke to send information back to our UI thread with a delegate
                //if we were to try to access the text box on the UI thread directly from a different thread, there would be problems
                this.Invoke(new poplateTextBoxDelegate(populateTextBox), new object[] { count.ToString() });
                Thread.Sleep(1000);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            workerFunctionDelegate w = workerFunction;
            w.BeginInvoke(3, null, null);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            workerFunction(3);
        }

        //DrawingContext drawingContext;
        System.Drawing.Pen pen;
        // Create a DrawingVisual that contains a rectangle.
       /* private DrawingVisual CreateDrawingVisualRectangle()
        {
            DrawingVisual drawingVisual = new DrawingVisual();

            // Retrieve the DrawingContext in order to create new drawing content.
            drawingContext = drawingVisual.RenderOpen();

            // Create a rectangle and draw it in the DrawingContext.
            // Draw a transparent background to set the render size
            drawingContext.DrawRectangle(Brushes.Black, (Pen)null, new Rect(0,0,100,100));
            // Persist the drawing content.
            // dc.Close();

            pen = new System.Drawing.Pen( Brushes.Red, 2);
            return drawingVisual;
        }*/
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Panel p = (Panel)sender;
            System.Drawing.Graphics g = e.Graphics;

            g.FillRectangle(System.Drawing.Brushes.Black, p.DisplayRectangle);

            System.Drawing.Point[] points = new System.Drawing.Point[4];

            points[0] = new System.Drawing.Point(0, 0);
            //points[1] = new System.Drawing.Point(0, p.Height);
            points[2] = new System.Drawing.Point(p.Width, p.Height);
            //points[3] = new System.Drawing.Point(p.Width, 0);

            g.DrawLines(pen, points);
        }
        private void btnDraw_Click(object sender, EventArgs e)
        {
            //CreateDrawingVisualRectangle();
           // Graphics g = p.CreateGraphics();
            //panel1.DrawToBitmap(null, new System.Drawing.Rectangle(0, 0, 200, 200));
            // Code goes here
           // drawingContext.DrawLine(pen, new Point(0, 0), new Point(20, 20));
        }
    }
}
