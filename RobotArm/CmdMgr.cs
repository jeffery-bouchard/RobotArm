using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RobotArm
{
    //command manager class used to queue commands sent to MCU
    public class CmdMgr
    {
        private readonly List<ICommand> Queue;
        private int Proximity;
        private bool _objectDetected;
        public Thread thread;
        private const int Pass = 500;
        private const int Fail = 501;
        private const int GripperMask = 0b_0110_0000;
        private const int ElbowMask = 0b_0000_0000;
        private const int ShoulderMask = 0b_0010_0000;
        private const int BaseMask = 0b_0100_0000;

        //constructor
        public CmdMgr()
        {
            Queue = new List<ICommand>();
            _objectDetected = false;
            MainWindow.Log("CmdMgr - CmdMgr - Command manager instantiated");
        }

        //object detected
        public bool ObjectDetected
        {
            get
            {
                return _objectDetected;
            }
        }

        //queue count (length)
        public int QueueCnt
        {
            get
            {
                return Queue.Count;
            }
        }

        //last command type in queue
        public string QueueLast
        {
            get
            {
                return Queue[Queue.Count - 1].GetType().ToString();
            }
        }

        //add command to queue
        public bool AddCmd(string cmd, int value = 0)
        {
            bool status;

            MainWindow.Log("CmdMgr - AddCmd - received request for command and value " + cmd + ", " + value.ToString());

            switch (cmd)
            {
                //stop actuator movement
                case "stop":
                    MainWindow.Log("CmdMgr - AddCmd - adding stop");
                    Queue.Clear();
                    Queue.Add(new Stop());
                    status = true;
                    break;
                //read proximity sensor
                case "proximity":
                    MainWindow.Log("CmdMgr - AddCmd - adding proximity");
                    Queue.Add(new ReadProximity());
                    status = true;
                    break;
                //set base actuator position
                case "base":
                    MainWindow.Log("CmdMgr - AddCmd - adding proximity");
                    Queue.Add(new ReadProximity());
                    MainWindow.Log("CmdMgr - AddCmd - adding base");
                    Queue.Add(new SetActuator(BaseMask, value));
                    status = true;
                    break;
                //set elbow actuator position
                case "elbow":
                    MainWindow.Log("CmdMgr - AddCmd - adding proximity");
                    Queue.Add(new ReadProximity());
                    MainWindow.Log("CmdMgr - AddCmd - adding elbow");
                    Queue.Add(new SetActuator(ElbowMask, value));
                    status = true;
                    break;
                //set gripper actuator position
                case "gripper":
                    MainWindow.Log("CmdMgr - AddCmd - adding proximity");
                    Queue.Add(new ReadProximity());
                    MainWindow.Log("CmdMgr - AddCmd - adding gripper");
                    Queue.Add(new SetActuator(GripperMask, value));
                    status = true;
                    break;
                //set shoulder actuator position
                case "shoulder":
                    MainWindow.Log("CmdMgr - AddCmd - adding proximity");
                    Queue.Add(new ReadProximity());
                    MainWindow.Log("CmdMgr - AddCmd - adding shoulder");
                    Queue.Add(new SetActuator(ShoulderMask, value));
                    status = true;
                    break;
                //set wait command
                case "wait":
                    MainWindow.Log("CmdMgr - AddCmd - adding wait");
                    Queue.Add(new Wait(value));
                    status = true;
                    break;
                //invalid command
                default:
                    MainWindow.Log("CmdMgr - AddCmd - invalid command");
                    status = false;
                    break;
            }
            return status;
        }

        //global stop
        public bool Stop()
        {
            MainWindow.Log("CmdMgr - Stop - adding stop");
            Queue.Clear();
            Queue.Add(new Stop());
            return true;
        }

        //start new thread for queue
        public void StartQueue()
        {
            MainWindow.Log("CmdMgr - StartQueue - queue thread starting");
            thread = new Thread(new ThreadStart(OnStart))
            {
                IsBackground = true
            };
            thread.Start();
        }

        //stop queue thread
        public void StopQueue()
        {
            thread.Abort();
            MainWindow.Log("CmgMgr - StopQueue - queue thread stopped");
        }

        //process items in queue
        public void OnStart()
        {
            MainWindow.Log("CmdMgr - OnStart - queue thread started");
            int status;
            while (true)
            {
                for (int i = 0; i < Queue.Count; i++)
                {
                    int index = i + 1;
                    MainWindow.Log("CmdMgr - OnStart - queue index is " + index.ToString());
                    string type = Queue[i].GetType().ToString();

                    MainWindow.Log("CmdMgr - OnStart - processing command " + type);
                    status = Queue[i].Execute();

                    //stop movement
                    if (type == "RobotArm.Stop")
                    {
                        if (status == Pass)
                        {
                            MainWindow.Log("CmdMgr - OnStart - Stop passed");
                        }
                        else if (status == Fail)
                        {
                            MainWindow.Log("CmdMgr - OnStart - Stop failed - ERROR");
                        }
                        else
                        {
                            MainWindow.Log("CmdMgr - OnStart - Stop invalid response - ERROR");
                        }
                    }
                    //wait
                    else if (type == "RobotArm.Wait")
                    {
                        if (status == Pass)
                        {
                            MainWindow.Log("CmdMgr - OnStart - wait passed");
                        }
                        else if (status == Fail)
                        {
                            MainWindow.Log("CmdMgr - OnStart - wait failed - ERROR");
                        }
                        else
                        {
                            MainWindow.Log("CmdMgr - OnStart - wait invalid response - ERROR");
                        }
                    }
                    //read proximity sensor
                    else if (type == "RobotArm.ReadProximity") 
                    {
                        if (status == Fail)
                        {
                            MainWindow.Log("CmdMgr - OnStart - ReadProximity failed - ERROR");
                        }
                        else if (status >= 0 && status <= 255)
                        {
                            
                            MainWindow.Log("CmdMgr - OnStart - ReadProximity passed with measurement of " + status.ToString());
                            int ProximityLimit = MainWindow.OAR.Proximity.Limit;

                            MainWindow.Log("CmdMgr - OnStart - proximity limit is " + ProximityLimit.ToString());
                            //convert sensor reading to proximity value
                            if      (status >= 200) { Proximity = 5; }
                            else if (status >= 170) { Proximity = 6; }
                            else if (status >= 151) { Proximity = 7; }
                            else if (status >= 135) { Proximity = 8; }
                            else if (status >= 120) { Proximity = 9; }
                            else if (status >= 108) { Proximity = 10; }
                            else if (status >= 99)  { Proximity = 11; }
                            else if (status >= 92)  { Proximity = 12; }
                            else if (status >= 86)  { Proximity = 13; }
                            else if (status >= 81)  { Proximity = 14; }
                            else if (status >= 77)  { Proximity = 15; }
                            else if (status >= 72)  { Proximity = 16; }
                            else if (status >= 68)  { Proximity = 17; }
                            else if (status >= 65)  { Proximity = 18; }
                            else if (status >= 63)  { Proximity = 19; }
                            else if (status >= 60)  { Proximity = 20; }
                            else if (status >= 57)  { Proximity = 21; }
                            else if (status >= 54)  { Proximity = 22; }
                            else if (status >= 52)  { Proximity = 23; }
                            else if (status >= 51)  { Proximity = 24; }
                            else if (status >= 50)  { Proximity = 25; }
                            else if (status >= 47)  { Proximity = 26; }
                            else if (status >= 45)  { Proximity = 27; }
                            else if (status >= 44)  { Proximity = 28; }
                            else if (status >= 43)  { Proximity = 29; }
                            else if (status >= 42)  { Proximity = 30; }
                            else                    { Proximity = 31; }

                            MainWindow.Log("CmdMgr - OnStart - Proximity is " + Proximity.ToString());

                            //proximity greater than limit (used for object avoidance)
                            if (Proximity > ProximityLimit)
                            {
                                if (MainWindow.OAR.Proximity.ObjectAvoidance == true)
                                {
                                    MainWindow.SetObjAvoidLED("green");
                                    _objectDetected = false;
                                    MainWindow.Log("CmdMgr - OnStart - Proximity " + Proximity.ToString() + " is greater than limit " + ProximityLimit.ToString());
                                }
                                else
                                {
                                    MainWindow.SetObjAvoidLED("grey");
                                    MainWindow.Log("CmdMgr - OnStart - object avoidance is off, no object detected");
                                }
                            }
                            //no object within proximity limit
                            else
                            {
                                _objectDetected = true;
                                MainWindow.Log("CmdMgr - OnStart - Proximity " + Proximity.ToString() + " is less than limit " + ProximityLimit.ToString());
                                MainWindow.Log("CmdMgr - OnStart - Object detected within limit");

                                if (MainWindow.OAR.Proximity.ObjectAvoidance == true) 
                                {
                                    MainWindow.SetObjAvoidLED("red");
                                    MainWindow.Log("CmdMgr - OnStart - object avoidance is on, inserting ReadProximity command into queue");
                                    Queue.Insert(i + 1, new ReadProximity());
                                }
                                else
                                {
                                    MainWindow.SetObjAvoidLED("grey");
                                    MainWindow.Log("CmdMgr - OnStart - object avoidance is off, ignoring object");
                                }
                            }
                        }
                        else
                        {
                            MainWindow.Log("CmdMgr - OnStart - ReadProximity invalid response - ERROR");
                        }
                    }
                    //set actuator
                    else if (type == "RobotArm.SetActuator")
                    {
                        if (status == Pass)
                        {
                            MainWindow.Log("CmdMgr - OnStart - SetActuator passed");
                        }
                        else if (status == Fail)
                        {
                            MainWindow.Log("CmdMgr - OnStart - SetActuator failed - ERROR");
                        }
                        else
                        {
                            MainWindow.Log("CmdMgr - OnStart - SetActuator invalid response - ERROR");
                        }
                    }
                    //invalid command
                    else
                    {
                        MainWindow.Log("CmdMgr - OnStart - invalid command type - ERROR");
                    }
                    //clear queue
                    if (i == Queue.Count-1)
                    {
                        MainWindow.Log("CmdMgr - OnStart - queue count is " + Queue.Count.ToString());
                        Queue.Clear();
                        MainWindow.Log("CmdMgr - OnStart - end of list, clearing queue");
                        MainWindow.Log("CmdMgr - OnStart - queue count is " + Queue.Count.ToString());
                    }
                    //log queue count
                    else
                    {
                        MainWindow.Log("CmdMgr - OnStart - queue count is " + Queue.Count.ToString());
                    }
                };
            }
        }
    }
}
