using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotArm
{
    //sensor class for proximity sensor
    public class Sensor
    {
        private int _limit;
        private readonly string _id;
        private bool _objAvoidance;

        //constructor
        public Sensor(string id, int limit, bool objAvoidance)
        {
            _id = id;
            _limit = limit;
            _objAvoidance = objAvoidance;
            MainWindow.Log("Sensor - Sensor - instantiating sensor");
        }

        //set sensor limit
        public int Limit
        {
            get
            {
                return _limit;
            }
            set
            {

                if (value > 4 && value < 31)
                {
                    _limit = value;
                }
                else
                {
                    MainWindow.Log("Sensor - Limit - value invalid - ERROR");
                }
            }
        }

        //object avoidance property
        public bool ObjectAvoidance
        {
            get
            {
                return _objAvoidance;
            }
            set
            {
                _objAvoidance = value;
            }
        }

        //evaluate the proximity sensor reading
        public bool EvalProximity()
        {
            MainWindow.Manager.AddCmd(_id);
            return true;
        }
    }
}
