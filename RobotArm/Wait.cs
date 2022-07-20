using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RobotArm
{
    //wait command class
    class Wait : ICommand
    {
        private int _duration;
        private const int pass = 500;
        private const int fail = 501;

        //constructor
        public Wait(int duration)
        {
            _duration = duration;
        }

        //execute wait command
        public int Execute()
        {
            int value = _duration * 1000;
            MainWindow.Log("Wait - Execute - waiting for " + _duration.ToString() + " seconds");
            Thread.Sleep(value);

            return pass;
        }
    }
}
