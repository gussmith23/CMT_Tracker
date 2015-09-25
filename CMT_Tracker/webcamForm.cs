using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;

namespace CMT_Tracker
{
    public partial class webcamForm : Form
    {
        private CMT mTracker;
        private Rectangle mDefinedROI;

        private Capture capture;
        private bool captureInProgress;

        private int boxwidth = 100;
        private Rectangle box = Rectangle.Empty;
        private Point BoxPosition = Point.Empty;
        private Size BoxSize = new Size(100,100);

        public webcamForm()
        {
            InitializeComponent();

            mTracker = new CMT();

            try
            {
                capture = new Capture();
                captureInProgress = true;
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e.StackTrace);
            }
            Application.Idle += ProcessFrame;
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }


        private void ProcessFrame(object sender, EventArgs arg) 
        {
            if (capture != null)
            {
                Image<Bgr, Byte> ImageFrame = capture.QueryFrame();

                if (mTracker.Initialized)
                {
                    mTracker.ProcessFrame(new Image<Bgr, Byte>((Bitmap)pictureBox1.Image), mDefinedROI);

                    for (int i = 0; i < mTracker.TrackedKeypoints.Count; i++)
                    {
                        ImageFrame.Draw(new CircleF(mTracker.ActiveKeypointsF[i], 3), new Bgr(255, 255, 255), 1);   // White
                    }
                    for (int i = 0; i < mTracker.Inliers.Count; i++)
                    {
                        ImageFrame.Draw(new CircleF(mTracker.Inliers[i], 3), new Bgr(255, 0, 0), 1);   // Blue
                    }
                    for (int i = 0; i < mTracker.Outliers.Count; i++)
                    {
                        ImageFrame.Draw(new CircleF(mTracker.Outliers[i], 3), new Bgr(0, 0, 255), 1);   // Red
                    }
                    ImageFrame.Draw(new CircleF(mTracker.CenterF, 4), new Bgr(Color.Green), -1); 
                }




                if (BoxPosition != Point.Empty) ImageFrame.Draw(new Rectangle(BoxPosition, BoxSize), new Bgr(Color.DarkOrange), 2);
                pictureBox1.Image = ImageFrame.ToBitmap();
            }

        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            var pos = pictureBox1.PointToClient(Cursor.Position);
            BoxPosition = new Point(pos.X - BoxSize.Width / 2, BoxPosition.Y = pos.Y - BoxSize.Height/2);
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            BoxPosition = Point.Empty;
        }

        private void webcamForm_MouseWheel(object sender, MouseEventArgs e)
        {
            BoxSize.Width += e.Delta/10;
            BoxSize.Height += e.Delta / 10;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            mDefinedROI = new Rectangle(BoxPosition, BoxSize);
            mTracker.Initialize(new Image<Bgr,Byte>((Bitmap)pictureBox1.Image), mDefinedROI);
        }

    }
}
