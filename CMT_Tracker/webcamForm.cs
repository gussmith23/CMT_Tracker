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

        private Capture capture;
        private bool captureInProgress;

        private int boxwidth = 100;

        public webcamForm()
        {
            InitializeComponent();

            capture = new Capture();
            captureInProgress = true;
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
            Image<Bgr, Byte> ImageFrame = capture.QueryFrame();
            ImageFrame.Draw(new Rectangle(0, 0, 100, 100), new Bgr(Color.DarkOrange), 2);
            imageBox1.Image = ImageFrame; 
        }

        private void imageBox1_MouseMove(object sender, MouseEventArgs e)
        {
            var pos = imageBox1.PointToClient(Cursor.Position);
            Image<Bgr, Byte> image = (Image<Bgr, Byte>)imageBox1.Image;
            if (image != null) image.Draw(new Rectangle(pos, new Size(boxwidth, boxwidth)), new Bgr(Color.DarkOrange), 2);
        }
    }
}
