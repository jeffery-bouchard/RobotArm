using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotArm
{
    //robot class to organize devices (actuators and sensors)
    public class Robot
    {
        public Actuator Gripper;
        public Actuator Elbow;
        public Actuator Shoulder;
        public Actuator Base;
        public Sensor Proximity;

        //constructor
        public Robot()
        {
            Gripper = new Actuator("gripper", 16, 5, 31, 16, false);
            Elbow = new Actuator("elbow", 16, 3, 25, 16, false);
            Shoulder = new Actuator("shoulder", 16, 6, 31, 16, false);
            Base = new Actuator("base", 16, 0, 31, 16, false);
            Proximity = new Sensor("proximity", 12, true);

            MainWindow.Log("Robot - Robot - Robot Instantiated");
        }

        //global home
        public bool GoHome()
        {
            MainWindow.Log("Robot - GoHome - Sending robot home");
            Gripper.GoHome();
            Elbow.GoHome();
            Shoulder.GoHome();
            Base.GoHome();
            return true;
        }

        //global stop
        public bool Stop()
        {
            MainWindow.Log("Robot - Stop - Stopping robot");
            MainWindow.Manager.Stop();
            return true;
        }

    }
}
