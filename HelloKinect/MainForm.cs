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
            int port = int.Parse(this.serverPortTextBox.Text);
            string address = this.serverAddressTextBox.Text;
            this.serverPortTextBox.Enabled = false;

            btnStartKinectAsync(address, port);
        }

        private async void btnStartKinectAsync(string address, int port)
        {
            await System.Threading.Tasks.Task.Run(() => btnStartKinectTask(address, port));
        }
        private async Task btnStartKinectTask(string address, int port)
        {
            KinectAPI.startKinect(address, port);

            KinectAPI.debugger.txtDebugger = txtSkeletonData;
            string skeletonData = KinectAPI.getSkeletonData();
        }

        public void drawBone() {

        }
        public void drawJoint() {

        }
    }
}
