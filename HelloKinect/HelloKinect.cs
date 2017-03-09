using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Kinect;


namespace HelloKinect
{
    public class HelloKinect
    {
        private string greeting = "Hello World";

        public HelloKinect()
        {
            Console.WriteLine("Hello Kinnect starting...");
        }

        public string getGreeting()
        {
            Console.WriteLine(greeting);
            return greeting;
        }
    }
}
