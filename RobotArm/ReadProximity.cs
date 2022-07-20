using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotArm
{
    //read proximity sensor class
    public class ReadProximity : ICommand
    {
        private string _message;
        private const int fail = 501;
        private int _measurement;
        private readonly int _limit;

        public string Message
        {
            get
            {
                return _message;
            }
        }

        public int Measurement
        {
            get
            {
                return _measurement;
            }
        }
        
        //read proximity sensor
        public int Execute()
        {
            MainWindow.Log("ReadProximity - Execute - executing command");
            //declare vars
            const byte Command = 0xE0;
            int result;
            byte[] buffer = new byte[1];

            //write command to serial port and read returned result
            MainWindow.Log("ReadProximity - Execute - sending command " + Command.ToString());
            MainWindow.Comm.Write(new byte[] { Command }, 0, 1);
            MainWindow.Log("ReadProximity - Execute - command sent, reading result");
            result = MainWindow.Comm.Read(buffer, 0, 1);
            MainWindow.Log("ReadProximity - Execute - result is " + result.ToString());
            if (result == 1)
            {
                //store proximity reading
                try
                {
                    _measurement = (int)buffer[0];
                    MainWindow.Log("ReadProximity - Execute - measurement is " + _measurement.ToString());
                    _message = "";
                    return _measurement;
                }
                //invalid proximity reading
                catch
                {
                    _message = "Invalid proximity reading";
                    MainWindow.Log("ReadProximity - Execute - invalid proximity reading");
                    return fail;
                }
            }
            //invalid byte count in command, only one allowed
            else
            {
                _message = "Incorrect number of bytes read";
                MainWindow.Log("ReadProximity - Execute - incorrect number of bytes read - ERROR");
                return fail;
            }
        }
    }
}
