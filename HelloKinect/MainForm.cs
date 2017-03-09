using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HelloKinect
{
    public partial class MainForm : Form, IKinectRender
    {
        public KinectAPI KinectAPI { get; set; }
        public HelloKinect HelloKinect { get; set; }

        public MainForm()
        {
            InitializeComponent();
            InitializeKinect();
        }
        private void InitializeKinect() {
            this.KinectAPI = new KinectAPI(this);
        }

        private void btnStartKinect(object sender, EventArgs e)
        {
            btnStartKinectAsync();
        }

        private async void btnStartKinectAsync()
        {
            await System.Threading.Tasks.Task.Run(() => btnStartKinectTask());
        }
        private async Task btnStartKinectTask()
        {
            KinectAPI.startKinect();
            KinectAPI.debugger.txtDebugger = txtSkeletonData;
            string skeletonData = KinectAPI.getSkeletonData();
        }

        public void drawBone() {

        }
        public void drawJoint() {

        }
    }
}
