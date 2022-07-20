using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace RobotArm
{
    //set actuator class for motors
    public class SetActuator : ICommand
    {
        private readonly int _mask;
        private string _message;
        private byte _command;
        private readonly int _position;
        private const int pass = 500;
        private const int fail = 501;

        public string Message
        {
            get
            {
                return _message;
            }
        }

        public int Mask
        {
            get
            {
                return _mask;
            }
        }

        public int Position
        {
            get
            {
                return _position;
            }
        }


        //set actuator position
        public SetActuator(int mask, int position)
        {
            _mask = mask;
            _position = position;
            MainWindow.Log("SetActuator - SetActuator - Instantiated with position of " + _position.ToString());
        }

        //execute command (send to MCU)
        public int Execute()
        {
            //validate position
            if(_position < 0 || _position > 31)
            {
                _message = "Invalid position.";
                MainWindow.Log("SetActuator - Execute - invalid position - ERROR");
                return fail;
            }
            else
            {
                MainWindow.Log("SetActuator - Execute - executing command");
                //declare vars
                int result;
                byte[] buffer = new byte[1];

                //OR bit mask with position bits and convert from int to byte
                int MaskedPosition = _mask | _position;
                MainWindow.Log("SetActuator - Execute - masked position is " + MaskedPosition.ToString());
                _command = Convert.ToByte(MaskedPosition);

                //write command to serial port and read returned result
                MainWindow.Log("SetActuator - Execute - sending command " + _command.ToString());
                MainWindow.Comm.Write(new byte[] { _command }, 0, 1);
                MainWindow.Log("SetActuator - Execute - command sent, reading result");

                result = MainWindow.Comm.Read(buffer, 0, 1);
                MainWindow.Log("SetActuator - Execute - result is " + result.ToString());
                if (result == 1)
                {
                    //MCU received and processed command successfully
                    if(buffer[0] == (byte)0x00)
                    {
                        MainWindow.Log("SetActuator - Execute - MCU processed command successfully");
                        _message = "";
                        return pass;
                    }
                    //MCU failed to receive and process command
                    else
                    {
                        MainWindow.Log("SetActuator - Execute - MCU could not process command - ERROR");
                        _message = "MCU could not process command";
                        return fail;
                    }
                }
                //invalid byte count in command, only one allowed
                else
                {
                    MainWindow.Log("SetActuator - Execute - incorrect number of bytes received from MCU - ERROR");
                    _message = "Incorrect number of bytes read";
                    return fail;
                }
            }         
        }
    }
}
