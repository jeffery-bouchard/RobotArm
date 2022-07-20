using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotArm
{

    //actuator class used for managing motor position
    public class Actuator
    {
        private readonly string _id;
        private readonly int _home;
        private readonly int _min;
        private readonly int _max;
        private readonly bool _inverted;
        private int _position;
       
        //unique identifier
        public string ID
        {
            get
            {
                return _id;
            }
        }

        //home position
        public int Home
        {
            get
            {
                return _home;
            }
        }

        //minimum position
        public int Min
        {
            get
            {
                return _min;
            }
        }

        //maximum position
        public int Max
        {
            get
            {
                return _max;
            }
        }

        //inverter, or non-inverted (normal) movement
        public bool Inverted
        {
            get
            {
                return _inverted;
            }
        }

        //actuator position
        public int Position
        {
            //return current position
            get
            {
                return _position;
            }

            //increment position to desired setting if valid
            set
            {
                MainWindow.Log("Actuator - Position - set position request received: " + value.ToString());
                
                //valid position
                if (value >= _min && value <= _max)
                {
                    //decrement position
                    if (value < _position)
                    {
                        while(value < _position)
                        {
                            _position--;
                            MainWindow.Log("Actuator - Position - adding set position command to command manager.");
                            MainWindow.Manager.AddCmd(_id, _position);
                        }
                    }
                    //increment position
                    else if (value > _position)
                    {
                        while (value > _position)
                        {
                            _position++;
                            MainWindow.Log("Actuator - Position - adding set position command to command manager.");
                            MainWindow.Manager.AddCmd(_id, _position);
                        }
                    }
                    //no change
                    else
                    {
                        _position = value;
                        MainWindow.Log("Actuator - Position - adding set position command to command manager.");
                        MainWindow.Manager.AddCmd(_id, _position);
                    }

                    
                }
                //invalid position
                else
                {
                    MainWindow.Log("Actuator - Position - invalid position");
                }
            }
        }

        //constructor
        public Actuator(string id, int home, int min, int max, int position, bool inverted)
        {
            _id = id;
            _home = home;
            _min = min;
            _max = max;
            _position = position;
            _inverted = inverted;
            MainWindow.Log("Actuator - Actuator - instantiating actuator");
        }

        //increment actuator position
        public bool Increment()
        {
            //non-inverted (normal)
            if (_inverted == false)
            {
                if (_position < _max)
                {
                    _position++;
                    MainWindow.Log("Actuator - Increment - Adding set position command to command manager");
                    MainWindow.Manager.AddCmd(_id, _position);
                    return true;
                }
                else
                {
                    MainWindow.Log("Actuator - Increment - Position at maximum value");
                    return false;
                }
            }
            //inverted
            else
            {
                if (_position > _min)
                {
                    _position--;
                    MainWindow.Log("Actuator - Increment - Adding set position command to command manager");
                    MainWindow.Manager.AddCmd(_id, _position);
                    return true;
                }
                else
                {
                    MainWindow.Log("Actuator - Increment - Position at minimum value");
                    return false;
                }
            }
        }

        //decrement actuator position
        public bool Decrement()
        {
            //non-inverted (normal)
            if (_inverted == false)
            {
                if (_position > _min)
                {
                    _position--;
                    MainWindow.Log("Actuator - Decrement - Adding set position command to command manager");
                    MainWindow.Manager.AddCmd(_id, _position);
                    return true;
                }
                else
                {
                    MainWindow.Log("Actuator - Decrement - Position at minimum value");
                    return false;
                }
            }
            //inverted
            else
            {
                if (_position < _max)
                {
                    _position++;
                    MainWindow.Log("Actuator - Decrement - Adding set position command to command manager");
                    MainWindow.Manager.AddCmd(_id, _position);
                    return true;
                }
                else
                {
                    MainWindow.Log("Actuator - Decrement - Position at maxiumum value");
                    return false;
                }
            }

        }

        //send actuator to home position
        public bool GoHome()
        {
            if (_home < _position)
            {
                while (_home < _position)
                {
                    _position--;
                    MainWindow.Log("Actuator - GoHome - Adding set position command to command manager");
                    MainWindow.Manager.AddCmd(_id, _position);
                }
            }
            else if (_home > _position)
            {
                while (_home > _position)
                {
                    _position++;
                    MainWindow.Log("Actuator - GoHome - Adding set position command to command manager");
                    MainWindow.Manager.AddCmd(_id, _position);
                }
            }
            else
            {
                _position = _home;
                MainWindow.Log("Actuator - GoHome - Adding set position command to command manager");
                MainWindow.Manager.AddCmd(_id, _position);
            }

            return true;
        }

        //stop actuator movement
        public bool Stop()
        {
            MainWindow.Log("Actuator - Stop - Adding stop command to command manager");
            MainWindow.Manager.Stop();
            return true;
        }
    }
}
