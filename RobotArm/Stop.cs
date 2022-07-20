using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace RobotArm
{
    //stop class (stop motor)
    public class Stop : ICommand
    {
        private string _message;
        private const int pass = 500;
        private const int fail = 501;

        public string Message
        {
            get
            {
                return _message;
            }
        }
        
        //execute stop command
        public int Execute()
        {
            //declare vars
            const byte Command = 0xFF;
            int result;
            byte[] buffer = new byte[1];

            //write command to serial port and read returned result
            MainWindow.Comm.Write(new byte[] { Command }, 0, 1);
            result = MainWindow.Comm.Read(buffer, 0, 1);
            if (result == 1)
            {
                //MCU received and processed command successfully
                if (buffer[0] == (byte)0x00)
                {
                    _message = "";
                    return pass;
                }
                //MCU failed to receive and process command
                else
                {
                    _message = "MCU could not process command";
                    return fail;
                }
            }
            //invalid byte count in command, only one allowed
            else
            {
                _message = "Incorrect number of bytes read";
                return fail;
            }
        }
    }
}
