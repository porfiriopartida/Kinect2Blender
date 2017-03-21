using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Kinect;
using System.IO;
using System.Threading;
using System.Windows.Media;
using System.Windows;
using System.Net.Sockets;
using System.Text;

namespace HelloKinect
{
    public class KinectAPI
    {
        public VisualDebugger debugger { get;  }

        //Kinect Vars
        private Skeleton[] skeletonData = new Skeleton [20];
        private KinectSensor sensor;
        /**
         * In charge of receiving the drawing calls.
         * */
        private IKinectRender kinectDrawer;

        //Drawing vars
        private DrawingGroup drawingGroup;
        private DrawingImage imageSource;
        private const float RenderWidth = 640.0f;
        private byte[] colorPixels;
        private const float RenderHeight = 480.0f;
        private readonly Brush trackedJointBrush = new SolidColorBrush(Color.FromArgb(255, 68, 192, 68));
        private readonly Brush inferredJointBrush = Brushes.Yellow;
        private readonly Pen trackedBonePen = new Pen(Brushes.Green, 6);
        private readonly Pen inferredBonePen = new Pen(Brushes.Gray, 1);
        DrawingContext drawingContext;
        Rect rect = new Rect(new Point(0, 0), new Size(RenderWidth, RenderHeight));

        public KinectAPI(IKinectRender receiver)
        {
            this.kinectDrawer = receiver;
            debugger = new VisualDebugger();
        }
        // Create a DrawingVisual that contains a rectangle.
        private DrawingVisual CreateDrawingVisualRectangle()
        {
            DrawingVisual drawingVisual = new DrawingVisual();

            // Retrieve the DrawingContext in order to create new drawing content.
            drawingContext = drawingVisual.RenderOpen();

            // Create a rectangle and draw it in the DrawingContext.
            // Draw a transparent background to set the render size
            drawingContext.DrawRectangle(Brushes.Black, (Pen)null, rect);
            // Persist the drawing content.
            // dc.Close();

            return drawingVisual;
        }
        string address;
        int port;
        private void sendTcpMessage(string str) {
            try
            {
                if (tcpclnt == null)
                {
                    return;
                }

                ASCIIEncoding asen = new ASCIIEncoding();
                Stream stm = tcpclnt.GetStream();

                byte[] ba = asen.GetBytes(str);
                stm.Write(ba, 0, ba.Length);
            }
            catch (Exception e)
            {
                if (tcpclnt != null)
                {
                    tcpclnt.Close();
                }

                Console.WriteLine("Error..... " + e.StackTrace);
            }

        }
        TcpClient tcpclnt;
        private void initClient() {
            try
            {
                tcpclnt = new TcpClient();
                Console.WriteLine("Connecting.....");
                tcpclnt.Connect(address, port);
            }
            catch (Exception e)
            {
                if (tcpclnt != null)
                {
                    tcpclnt.Close();
                    tcpclnt = null;
                }
                Console.WriteLine("Error..... " + e.StackTrace);
            }

        }
        public void startKinect(string address, int port)
        {
            this.address = address;
            this.port = port;

            initClient();

            CreateDrawingVisualRectangle();
            //this.drawingGroup = new DrawingGroup();
            //this.dc = this.drawingGroup.Open();
            if (this.sensor != null)
            {
                return;
            }

            foreach (var potentialSensor in KinectSensor.KinectSensors)
            {
                if (potentialSensor.Status == KinectStatus.Connected)
                {
                    this.sensor = potentialSensor;
                    break;
                }
            }

            if (this.sensor != null)
            {
                //this.sensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
                //this.sensor.ColorStream.Enable(ColorImageFormat.InfraredResolution640x480Fps30);
                this.sensor.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);
                this.sensor.SkeletonStream.Enable();
                this.sensor.SkeletonStream.TrackingMode = SkeletonTrackingMode.Seated;
                // enable returning skeletons while depth is in Near Range
                this.sensor.SkeletonStream.EnableTrackingInNearRange = true;

                skeletonData = new Skeleton[this.sensor.SkeletonStream.FrameSkeletonArrayLength]; // Allocate ST data
                this.sensor.SkeletonFrameReady += this.kinect_SkeletonFrameReady;
            }

            try
            {
                if (this.sensor != null)
                {
                    this.sensor.Start();
                }
            }
            catch (IOException)
            {
                this.sensor = null;
            }

            if (null == this.sensor)
            {
                debugger.WriteLine("Kinect failed to start...");
            }
            else
            {
                debugger.WriteLine("Kinect started...");
            }
        }
        private void EnableNearModeSkeletalTracking()
        {
            if (this.sensor != null && this.sensor.DepthStream != null && this.sensor.SkeletonStream != null)
            {
                this.sensor.DepthStream.Range = DepthRange.Near; // Depth in near range enabled
                this.sensor.SkeletonStream.EnableTrackingInNearRange = true; // enable returning skeletons while depth is in Near Range
                this.sensor.SkeletonStream.TrackingMode = SkeletonTrackingMode.Seated; // Use seated tracking
            }
        }

        private void kinect_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            //dc.DrawRectangle(Brushes.Black, null, new Rect(0.0, 0.0, RenderWidth, RenderHeight));
            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                // Open the Skeleton frame
                if (skeletonFrame != null && this.skeletonData != null) // check that a frame is available
                {
                    skeletonFrame.CopySkeletonDataTo(this.skeletonData); // get the skeletal information in this frame
                    if (this.sensor.SkeletonStream.TrackingMode == SkeletonTrackingMode.Seated)
                    {
                        DrawSeatedSkeletons(drawingContext);
                    }
                    else
                    {
                        DrawStandingSkeletons();
                    }
                }
            } // end using
        }

        private void DrawStandingSkeletons()
        {
            //DrawSeatedSkeletons();
        }

        bool canDraw = true;
        private async Task resetCanDraw()
        {
            canDraw = false;
            int milliseconds = 100;
            Thread.Sleep(milliseconds);
            canDraw = true;
        }

        private async void DrawSeatedSkeletons(DrawingContext drawingContext)
        {
            if (!canDraw) { return; }
            await System.Threading.Tasks.Task.Run(() => resetCanDraw());
            foreach(Skeleton skeleton in skeletonData) {
                if (skeleton != null && skeleton.TrackingState != SkeletonTrackingState.NotTracked)
                {
                 //   debugger.WriteLine("\n\r Skeleton: " + (skeleton.TrackingId));
                    //debugger.WriteLine("\n\r - - Position: (" + (skeleton.Position.X + ", " + skeleton.Position.Y + ", " + skeleton.Position.Z) + " ");
                    JointCollection jointCollection = skeleton.Joints;
                    BoneOrientationCollection boneOrientationCollection = skeleton.BoneOrientations;
                    foreach (Joint joint in jointCollection)
                    {
                        if (joint.TrackingState != JointTrackingState.NotTracked)
                        {
                            //Blender plugin accepted format
                            //jointName x y z 
                            string tcpMessage = joint.JointType.ToString() + " " + (joint.Position.X + " " + joint.Position.Y + " " + joint.Position.Z + " \n");
                            debugger.WriteLine("\n\r" + tcpMessage  + " ");
                            sendTcpMessage(tcpMessage);
                        }
                    }
                    
                    /*
                    if (skeleton.TrackingState == SkeletonTrackingState.Tracked)
                    {
                        DrawTrackedSkeletonJoints(skeleton.Joints);
                    }
                    else if (skeleton.TrackingState == SkeletonTrackingState.PositionOnly)
                    {
                        DrawSkeletonPosition(skeleton.Position);
                    }
                    */
                    /*
                    foreach (BoneOrientation boneOrientation in boneOrientationCollection)
                    {
                        Vector4 rotation = boneOrientation.AbsoluteRotation.Quaternion;
                        //DrawBonewithRotation(orientation.StartJoint, orientation.EndJoint, orientation.AbsoluteRotation.Quaternion);
                        debugger.WriteLine("\n\r - - - Bone (" + boneOrientation.StartJoint + " -> " + boneOrientation.EndJoint + ") Rotation: (" + (rotation.X + ", " + rotation.Y + ", " + rotation.Z + ", " + rotation.W) + ") ");
                    }
                    */

                    //debugger.WriteLine("\n\r\n\r==================\n\r\n\r");
                }
            }
        }
        private void DrawTrackedSkeletonJoints(JointCollection jointCollection)
        {
            // Render Head and Shoulders
            DrawBone(jointCollection[JointType.Head], jointCollection[JointType.ShoulderCenter]);
            DrawBone(jointCollection[JointType.ShoulderCenter], jointCollection[JointType.ShoulderLeft]);
            DrawBone(jointCollection[JointType.ShoulderCenter], jointCollection[JointType.ShoulderRight]);

            // Render Left Arm
            DrawBone(jointCollection[JointType.ShoulderLeft], jointCollection[JointType.ElbowLeft]);
            DrawBone(jointCollection[JointType.ElbowLeft], jointCollection[JointType.WristLeft]);
            DrawBone(jointCollection[JointType.WristLeft], jointCollection[JointType.HandLeft]);

            // Render Right Arm
            DrawBone(jointCollection[JointType.ShoulderRight], jointCollection[JointType.ElbowRight]);
            DrawBone(jointCollection[JointType.ElbowRight], jointCollection[JointType.WristRight]);
            DrawBone(jointCollection[JointType.WristRight], jointCollection[JointType.HandRight]);

            // Render other bones...
        }
        private void DrawBone(Joint jointFrom, Joint jointTo)
        {
            if (jointFrom.TrackingState == JointTrackingState.NotTracked ||
            jointTo.TrackingState == JointTrackingState.NotTracked)
            {
                return; // nothing to draw, one of the joints is not tracked
            }

            if (jointFrom.TrackingState == JointTrackingState.Inferred ||
            jointTo.TrackingState == JointTrackingState.Inferred)
            {
                DrawNonTrackedBoneLine(jointFrom.Position, jointTo.Position);  // Draw thin lines if either one of the joints is inferred
            }

            if (jointFrom.TrackingState == JointTrackingState.Tracked &&
            jointTo.TrackingState == JointTrackingState.Tracked)
            {
                DrawTrackedBoneLine(jointFrom.Position, jointTo.Position);  // Draw bold lines if the joints are both tracked
            }
        }
        private void DrawTrackedBoneLine(SkeletonPoint positionFrom, SkeletonPoint positionTo)
        {
            /*
            this.Dispatcher.Invoke(() =>
            {
                // Code goes here
                drawingContext.DrawLine(this.trackedBonePen, this.SkeletonPositionToScreen(positionFrom), this.SkeletonPositionToScreen(positionTo));
            });
            */
        }

        private void DrawNonTrackedBoneLine(SkeletonPoint positionFrom, SkeletonPoint positionTo)
        {
            // Code goes here
            drawingContext.DrawLine(this.inferredBonePen, this.SkeletonPositionToScreen(positionFrom), this.SkeletonPositionToScreen(positionTo));
        }

        private void DrawSkeletonPosition(SkeletonPoint skeletonPoint)
        {
            drawingContext.DrawEllipse(Brushes.Azure, null, this.SkeletonPositionToScreen(skeletonPoint), 2, 2);
        }

        private Point SkeletonPositionToScreen(SkeletonPoint skeletonPoint)
        {
            ColorImagePoint colorPoint = this.sensor.CoordinateMapper.MapSkeletonPointToColorPoint(skeletonPoint, ColorImageFormat.RgbResolution640x480Fps30);
            return new Point(colorPoint.X, colorPoint.Y);
        }

        public string getSkeletonData() {
            return skeletonData.ToString();
        }
    }
}
