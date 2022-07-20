using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.IO.Ports;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace RobotArm
{
    //main window class
    public partial class MainWindow : Window
    {
        public static SerialPort Comm;
        public static Robot OAR;
        public static CmdMgr Manager;
        public string PortName;
        private static string _status;
        private static bool Connected;
        private static ReaderWriterLock Lock;

        //constructor
        public MainWindow()
        {
            InitializeComponent();
            Lock = new ReaderWriterLock();
            Log("");
            Log("-----------------------------------------------");
            Log("MainWindow - MainWindow - Launching main window");
            Log("-----------------------------------------------");
            Comm = new SerialPort();
            OAR = new Robot();
            sldGripper.Minimum = OAR.Gripper.Min;
            sldElbow.Minimum = OAR.Elbow.Min;
            sldShoulder.Minimum = OAR.Shoulder.Min;
            sldBase.Minimum = OAR.Base.Min;
            sldGripper.Maximum = OAR.Gripper.Max;
            sldElbow.Maximum = OAR.Elbow.Max;
            sldShoulder.Maximum = OAR.Shoulder.Max;
            sldBase.Maximum = OAR.Base.Max;
            Manager = new CmdMgr();
            OAR.GoHome();
            UpdateGripper();
            UpdateElbow();
            UpdateShoulder();
            UpdateBase();
            Connected = false;
        }

        //status property
        public static string Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
            }
        }
        
        //write to log file
        public static void Log (string message)
        {
            string path = @"C:\temp\RobotArmLog.txt";
            string time = DateTime.Now.ToString();
            string line = time + ": " + message + Environment.NewLine;

            Lock.AcquireWriterLock(Timeout.Infinite);
            
            if (!File.Exists(path))
            {
                File.AppendAllText(path, line);
            }
            else
            {
                while (!IsFileReady(path)) { }
                File.AppendAllText(path, line);
            }

            Lock.ReleaseWriterLock();
        }

        //check if file is available
        public static bool IsFileReady(string filename)
        {

            FileInfo file = new FileInfo(filename);
            FileStream stream = null;
            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                return false;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            return true;
        }

        //increment gripper press
        private void IncGripperPress (object sender, RoutedEventArgs e)
        {
            Log("MainWindow - IncGripperPress - Incrementing gripper");
            OAR.Gripper.Increment();
            UpdateGripper();
            SetStatus("");
        }

        //decrement gripper press
        private void DecGripperPress (object sender, RoutedEventArgs e)
        {
            Log("MainWindow - DecGripperPress - Decrementing gripper");
            OAR.Gripper.Decrement();
            UpdateGripper();
            SetStatus("");
        }

        //increment elbow press
        private void IncElbowPress(object sender, RoutedEventArgs e)
        {
            Log("MainWindow - IncElbowPress - Incrementing elbow");
            OAR.Elbow.Increment();
            UpdateElbow();
            SetStatus("");
        }

        //decrement elbow press
        private void DecElbowPress(object sender, RoutedEventArgs e)
        {
            Log("MainWindow - DecElbowPress - Decrementing elbow");
            OAR.Elbow.Decrement();
            UpdateElbow();
            SetStatus("");
        }

        //increment shoulder press
        private void IncShoulderPress(object sender, RoutedEventArgs e)
        {
            Log("MainWindow - IncShoulderPress - Incrementing shoulder");
            OAR.Shoulder.Increment();
            UpdateShoulder();
            SetStatus("");
        }

        //decrement shoulder press
        private void DecShoulderPress(object sender, RoutedEventArgs e)
        {
            Log("MainWindow - DecShoulderPress - Decrementing shoulder");
            OAR.Shoulder.Decrement();
            UpdateShoulder();
            SetStatus("");
        }

        //increment base press
        private void IncBasePress(object sender, RoutedEventArgs e)
        {
            Log("MainWindow - IncBasePress - Incrementing base");
            OAR.Base.Increment();
            UpdateBase();
            SetStatus("");
        }

        //decrement base press
        private void DecBasePress(object sender, RoutedEventArgs e)
        {
            Log("MainWindow - DecBasePress - Decrementing base");
            OAR.Base.Decrement();
            UpdateBase();
            SetStatus("");
        }

        //keyboard press (keyboard shortcuts)
        private void KeyPress(object sender, KeyEventArgs e)
        {
            //up arrow (increment elbow)
            if (e.Key == Key.Up)
            {
                Log("MainWindow - KeyPress - Incrementing elbow");
                OAR.Elbow.Increment();
                UpdateElbow();
                SetStatus("");
            }
            //down arrow (decrement elbow)
            else if (e.Key == Key.Down)
            {
                Log("MainWindow - KeyPress - Decrementing elbow");
                OAR.Elbow.Decrement();
                UpdateElbow();
                SetStatus("");
            }
            //left arrow (decrement shoulder)
            else if (e.Key == Key.Left)
            {
                Log("MainWindow - KeyPress - Decrementing shoulder");
                OAR.Shoulder.Decrement();
                UpdateShoulder();
                SetStatus("");
            }
            //right arrow (increment shoulder)
            else if (e.Key == Key.Right)
            {
                Log("MainWindow - KeyPress - Incrementing shoulder");
                OAR.Shoulder.Increment();
                UpdateShoulder();
                SetStatus("");
            }
            //s button (increment gripper)
            else if (e.Key == Key.S)
            {
                Log("MainWindow - KeyPress - Incrementing gripper");
                OAR.Gripper.Increment();
                UpdateGripper();
                SetStatus("");
            }
            //x button (decrement gripper)
            else if (e.Key == Key.X)
            {
                Log("MainWindow - KeyPress - Decrementing gripper");
                OAR.Gripper.Decrement();
                UpdateGripper();
                SetStatus("");
            }
            //z button (decrement base)
            else if (e.Key == Key.Z)
            {
                Log("MainWindow - KeyPress - Decrementing base");
                OAR.Base.Decrement();
                UpdateBase();
                SetStatus("");
            }
            //c button (increment base)x
            else if (e.Key == Key.C)
            {
                Log("MainWindow - KeyPress - Incrementing base");
                OAR.Base.Increment();
                UpdateBase();
                SetStatus("");
            }

        }

        //gripper slider changed
        private void GripperDragCompleted (object sender, DragCompletedEventArgs e)
        {
            var slider = sender as Slider;
            double value = slider.Value;
            Log("MainWindow - GripperDragCompleted - Changing gripper position to " + value.ToString());
            OAR.Gripper.Position = (int)value;
            UpdateGripper("text");
            SetStatus("");
        }

        //elbow slider changed
        private void ElbowDragCompleted (object sender, DragCompletedEventArgs e)
        {
            var slider = sender as Slider;
            double value = slider.Value;
            Log("MainWindow - ElbowDragCompleted - Changing elbow position to " + value.ToString());
            OAR.Elbow.Position = (int)value;
            UpdateElbow("text");
            SetStatus("");
        }

        //shoulder slider changed
        private void ShoulderDragCompleted (object sender, DragCompletedEventArgs e)
        {
            var slider = sender as Slider;
            double value = slider.Value;
            Log("MainWindow - ShoulderDragCompleted - Changing shoulder position to " + value.ToString());
            OAR.Shoulder.Position = (int)value;
            UpdateShoulder("text");
            SetStatus("");
        }

        //base slider changed
        private void BaseDragCompleted (object sender, DragCompletedEventArgs e)
        {
            var slider = sender as Slider;
            double value = slider.Value;
            Log("MainWindow - BaseDragCompleted - Changing base position to " + value.ToString());
            OAR.Base.Position = (int)value;
            UpdateBase("text");
            SetStatus("");
        }

        //gripper text box value changed
        private void GripperTxtKeyDown (object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                string text = txtGripper.Text;
                try
                {
                    int value = int.Parse(text);
                    Log("MainWindow - GripperTxtKeyDown - Changing gripper position to " + value.ToString());
                    OAR.Gripper.Position = value;
                    UpdateGripper("slider");
                    SetStatus("");
                }
                catch
                {
                    Log("MainWindow - GripperTxtKeyDown - Invalid gripper position enterred - ERROR");
                    UpdateGripper();
                    SetStatus("Invalid position");
                }
            }
        }

        //elbow text box value changed
        private void ElbowTxtKeyDown (object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                string text = txtElbow.Text;
                try
                {
                    int value = int.Parse(text);
                    Log("MainWindow - ElbowTxtKeyDown - Changing elbow position to " + value.ToString());
                    OAR.Elbow.Position = value;
                    UpdateElbow("slider");
                    SetStatus("");
                }
                catch
                {
                    Log("MainWindow - ElbowTxtKeyDown - Invalid elbow position enterred - ERROR");
                    UpdateElbow();
                    SetStatus("Invalid position");
                }
            }
        }

        //shoulder text box value changed
        private void ShoulderTxtKeyDown (object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                string text = txtShoulder.Text;
                try
                {
                    int value = int.Parse(text);
                    Log("MainWindow - ShoulderTxtKeyDown - Changing shoulder position to " + value.ToString());
                    OAR.Shoulder.Position = value;
                    UpdateShoulder("slider");
                    SetStatus("");
                }
                catch
                {
                    Log("MainWindow - ShoulderTxtKeyDown - Invalid shoulder position enterred - ERROR");
                    UpdateShoulder();
                    SetStatus("Invalid position");
                }
            }
        }

        //base text box value changed
        private void BaseTxtKeyDown (object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                string text = txtBase.Text;
                try
                {
                    int value = int.Parse(text);
                    Log("Main Window - BaseTxtKeyDown - Changing base position to " + value.ToString());
                    OAR.Base.Position = value;
                    UpdateBase("slider");
                    SetStatus("");
                }
                catch
                {
                    Log("Main Window - BaseTxtKeyDown - Invalid base position enterred - ERROR");
                    UpdateBase();
                    SetStatus("Invalid position");
                }
            }
        }

        //home button pressed
        private void HomePress(object sender, RoutedEventArgs e)
        {
            Log("MainWindow - HomePress - Home button pressed");
            OAR.GoHome();
            UpdateGripper();
            UpdateElbow();
            UpdateShoulder();
            UpdateBase();
            SetStatus("");
        }
        
        //exit button pressed
        private void ExitPress(object sender, RoutedEventArgs e)
        {
            Log("MainWindow - ExitPress - Exit button pressed");
            try
            {
                Comm.Close();
                Log("MainWindow - ExitPress - serial port connection closed");
            }
            catch
            {
                Log("MainWindow - ExitPress - could not close serial port connection - ERROR");
            }
            Close();
        }
        
        //connect button pressed (connect to serial port)
        private void ConnectPress(object sender, RoutedEventArgs e)
        {
            Log("MainWindow - ConnectPress - Connect button pressed");
            bool? result;
            SelectPort sel = new SelectPort();
            string[] ports = SerialPort.GetPortNames();
            Array.Sort(ports);
            sel.boxPortName.ItemsSource = ports;
            result = sel.ShowDialog();

            if (result == true)
            {
                Log("MainWindow - ConnectPress -" + sel.Port + " selected");
                try
                {
                    PortName = sel.Port;
                    Comm.PortName = PortName;
                    Comm.BaudRate = 10417;
                    Comm.Parity = Parity.None;
                    Comm.DataBits = 8;
                    Comm.StopBits = StopBits.One;
                    Comm.Open();
                    Manager.StartQueue();
                    Connected = true;
                    SetStatus($"Connected to {PortName}");
                    _status = $"Connected to {PortName}";
                    Log("MainWindow - ConnectPress - Connected to " + PortName);
                }
                catch
                {
                    Log("MainWindow - ConnectPress - Connection failed - ERROR");
                    _status = "Disconnected";
                    SetStatus("Error: connection failed");
                }
            }
        }

        //disconnect button pressed (disconnect from serial port)
        private void DisconnectPress(object sender, RoutedEventArgs e)
        {
            Log("MainWindow - DisconnectPress - Disconnect button pressed");
            try
            {
                Comm.Close();
                Connected = false;
                SetStatus("Disconnected");
                _status = "Disconnected";
                Log("MainWindow - DisconnectPress - Disconnected from serial port");
            }
            catch
            {
                Log("MainWindow - DisconnectPress - Disconnection failed - ERROR");
                SetStatus("Error: disconnection failed");
            }
        }

        //hover over gripper motor
        private void GripperMouseEnter(object sender, MouseEventArgs e)

        {
            imgDiagram.Source = new BitmapImage(new Uri("Images/robot_gripper.png", UriKind.Relative));
        }

        //hover over elbow motor
        private void ElbowMouseEnter(object sender, MouseEventArgs e)

        {
            imgDiagram.Source = new BitmapImage(new Uri("Images/robot_elbow.png", UriKind.Relative));
        }

        //hover over shoulder motor
        private void ShoulderMouseEnter(object sender, MouseEventArgs e)

        {
            imgDiagram.Source = new BitmapImage(new Uri("Images/robot_shoulder.png", UriKind.Relative));
        }

        //hover over base motor
        private void BaseMouseEnter(object sender, MouseEventArgs e)

        {
            imgDiagram.Source = new BitmapImage(new Uri("Images/robot_base.png", UriKind.Relative));

        }

        //hover over non-motor GUI elements (default)
        private void ActuatorMouseLeave(object sender, MouseEventArgs e)
        {
            imgDiagram.Source = new BitmapImage(new Uri("Images/robot.png", UriKind.Relative));
        }

        //set status bar message
        public bool SetStatus(string status)
        {
            if (status == "")
            {
                txtStatus.Text = _status;
                return true;
            }
            else if (status.Length <= 50)
            {
                txtStatus.Text = status;
                return true;
            }
            else
            {
                txtStatus.Text = status.Substring(0, 46) + "...";
                return false;
            }
        }

        //update the gripper value
        private void UpdateGripper(string mode = "all")
        {
            int value = OAR.Gripper.Position;

            if (mode == "text")
            {
                txtGripper.Text = value.ToString();
            }
            else if (mode == "slider")
            {
                sldGripper.Value = value;
            }
            else
            {
                sldGripper.Value = value;
                txtGripper.Text = value.ToString();
            }

            Log("MainWindow - UpdateGripper - Gripper value has been updated to " + value.ToString());
        }

        //update elbow position
        private void UpdateElbow(string mode = "all")
        {
            int value = OAR.Elbow.Position;

            if (mode == "text")
            {
                txtElbow.Text = value.ToString();
            }
            else if (mode == "slider")
            {
                sldElbow.Value = value;
            }
            else
            {
                sldElbow.Value = value;
                txtElbow.Text = value.ToString();
            }

            Log("MainWindow - UpdateElbow - Elbow value has been updated to " + value.ToString());
        }

        //update shoulder position
        private void UpdateShoulder(string mode = "all")
        {
            int value = OAR.Shoulder.Position;

            if (mode == "text")
            {
                txtShoulder.Text = value.ToString();
            }
            else if (mode == "slider")
            {
                sldShoulder.Value = value;
            }
            else
            {
                sldShoulder.Value = value;
                txtShoulder.Text = value.ToString();
            }

            Log("MainWindow - UpdateShoulder - Shoulder value has been updated to " + value.ToString());
        }

        //update base position
        private void UpdateBase(string mode = "all")
        {
            int value = OAR.Base.Position;

            if (mode == "text")
            {
                txtBase.Text = value.ToString();
            }
            else if (mode == "slider")
            {
                sldBase.Value = value;
            }
            else
            {
                sldBase.Value = value;
                txtBase.Text = value.ToString();
            }

            Log("MainWindow - UpdateBase - Base value has been updated to " + value.ToString());
        }

        //object avoidance button press       
        private void ObjAvoidPress(object sender, RoutedEventArgs e)
        {
            bool detection = OAR.Proximity.ObjectAvoidance;
            detection = !detection;

            if (detection == true)
            {
                OAR.Proximity.ObjectAvoidance = true;
                txtObjAvoid.Text = OAR.Proximity.Limit.ToString();
                txtObjAvoid.IsReadOnly = false;
                SetObjAvoidLED("green");
                btnObjAvoidImage.Source = new BitmapImage(new Uri("Images/eye_open.png", UriKind.Relative));
            }
            else
            {
                OAR.Proximity.ObjectAvoidance = false;
                txtObjAvoid.Text = "OFF";
                txtObjAvoid.IsReadOnly = true;
                SetObjAvoidLED("grey");
                btnObjAvoidImage.Source = new BitmapImage(new Uri("Images/eye_closed.png", UriKind.Relative));
            }

            Log("MainWindow - ObjAvoidPress - object avoidance toggled");
        }

        //proximity limit text box value changed
        private void ObjAvoidTxtKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                string text = txtObjAvoid.Text;
                try
                {
                    int value = int.Parse(text);
                    if (value > 4 && value < 31)
                    {
                        Log("Main Window - ObjAvoidTxtKeyDown - Changing proximity limit to " + value.ToString());
                        OAR.Proximity.Limit = value;
                        SetStatus("");
                    }
                    else
                    {
                        Log("Main Window - ObjAvoidTxtKeyDown - Invalid proximity limit enterred - ERROR");
                        txtObjAvoid.Text = OAR.Proximity.Limit.ToString();
                        SetStatus("Invalid limit");
                    }
                }
                catch
                {
                    Log("Main Window - ObjAvoidTxtKeyDown - Invalid proximity limit enterred - ERROR");
                    txtObjAvoid.Text = OAR.Proximity.Limit.ToString();
                    SetStatus("Invalid limit");
                }

            }
        }

        //set object avoidance LED color
        public static bool SetObjAvoidLED(string color)
        {
            switch (color)
            {
                case "red":
                    {
                        Application.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            MainWindow myWindow = Application.Current.MainWindow as MainWindow;
                            myWindow.imgObjAvoidLED.Source = new BitmapImage(new Uri("Images/red_led.png", UriKind.Relative));
                        }));
                        return true;
                    }
                case "green":
                    {
                        Application.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            MainWindow myWindow = Application.Current.MainWindow as MainWindow;
                            myWindow.imgObjAvoidLED.Source = new BitmapImage(new Uri("Images/green_led.png", UriKind.Relative));
                        }));
                        return true;
                    }
                case "grey":
                    {
                        Application.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            MainWindow myWindow = Application.Current.MainWindow as MainWindow;
                            myWindow.imgObjAvoidLED.Source = new BitmapImage(new Uri("Images/grey_led.png", UriKind.Relative));
                        }));
                        return true;
                    }
                default:
                    {
                        return false;
                    }
            }
        }


        //new script press
        private void NewPress(object sender, RoutedEventArgs e)
        {
            txtScript.Document.Blocks.Clear();
            txtScriptPath.Clear();
        }

        //open script press
        private void OpenPress(object sender, RoutedEventArgs e)
        {
            string filename;
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "";
            dlg.DefaultExt = ".xaml";
            dlg.Filter = "Windows Markup File |*.xaml";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                filename = dlg.FileName;

                TextRange range;
                FileStream fStream;
                if (File.Exists(filename))
                {
                    range = new TextRange(txtScript.Document.ContentStart, txtScript.Document.ContentEnd);
                    fStream = new FileStream(filename, FileMode.OpenOrCreate);
                    range.Load(fStream, DataFormats.XamlPackage);
                    fStream.Close();
                    txtScriptPath.Text = filename;
                }
            }
        }

        //run script press
        private void RunPress(object sender, RoutedEventArgs e)
        {
            TextRange textRange = new TextRange(txtScript.Document.ContentStart, txtScript.Document.ContentEnd);
            string[] rtbLines = textRange.Text.Split('\n');
            foreach (string line in rtbLines)
            {
                string command = line.Trim();
                Log("MainWindow - RunPress - line read, " + command);

                //regular expressions used for command detection
                Regex gripper = new Regex(@"^gripper\(\d{1,2}\)");
                Regex elbow = new Regex(@"^elbow\(\d{1,2}\)");
                Regex shoulder = new Regex(@"^shoulder\(\d{1,2}\)");
                Regex bottom = new Regex(@"^base\(\d{1,2}\)");
                Regex proximity = new Regex(@"^proximity\(\d{1,2}\)");
                Regex wait = new Regex(@"^wait\(\d{1,2}\)");

                //set gripper position
                if (gripper.IsMatch(command))
                {
                    MainWindow.Log("MainWindow - RunPress - Found gripper command, " + command);
                    try
                    {
                        int position = int.Parse(command.Split('(', ')')[1]);
                        MainWindow.Log("MainWindow - RunPress - gripper position " + position.ToString());
                        OAR.Gripper.Position = position;
                        UpdateGripper();
                    }
                    catch
                    {
                        MainWindow.Log("MainWindow - RunPress - invalid gripper position");
                    }
                }
                //set elbow position
                else if (elbow.IsMatch(command))
                {
                    MainWindow.Log("MainWindow - RunPress - Found elbow command, " + command);
                    try
                    {
                        int position = int.Parse(command.Split('(', ')')[1]);
                        MainWindow.Log("MainWindow - RunPress - elbow position " + position.ToString());
                        OAR.Elbow.Position = position;
                        UpdateElbow();
                    }
                    catch
                    {
                        MainWindow.Log("MainWindow - RunPress - invalid elbow position");
                    }
                }
                //set shoulder position
                else if (shoulder.IsMatch(command))
                {
                    MainWindow.Log("MainWindow - RunPress - Found shoulder command, " + command);
                    try
                    {
                        int position = int.Parse(command.Split('(', ')')[1]);
                        MainWindow.Log("MainWindow - RunPress - shoulder position " + position.ToString());
                        OAR.Shoulder.Position = position;
                        UpdateShoulder();
                    }
                    catch
                    {
                        MainWindow.Log("MainWindow - RunPress - invalid gripper position");
                    }
                }
                //set bottom (base) position
                else if (bottom.IsMatch(command))
                {
                    MainWindow.Log("MainWindow - RunPress - Found base command, " + command);
                    try
                    {
                        int position = int.Parse(command.Split('(', ')')[1]);
                        MainWindow.Log("MainWindow - RunPress - base position " + position.ToString());
                        OAR.Base.Position = position;
                        UpdateBase();
                    }
                    catch
                    {
                        MainWindow.Log("MainWindow - RunPress - invalid gripper position");
                    }
                }
                //set proximity sensor limit
                else if (proximity.IsMatch(command))
                {
                    MainWindow.Log("MainWindow - RunPress - Found proximity command, " + command);
                    try
                    {
                        int value = int.Parse(command.Split('(', ')')[1]);
                        MainWindow.Log("MainWindow - RunPress - proximity limit " + value.ToString());
                        if (value > 4 && value < 31)
                        {
                            Log("Main Window - RunPress - Changing proximity limit to " + value.ToString());
                            txtObjAvoid.Text = value.ToString();
                        }
                        else
                        {
                            MainWindow.Log("MainWindow - RunPress - invalid proximity limit");
                        }
                    }
                    catch
                    {
                        MainWindow.Log("MainWindow - RunPress - invalid proximity limit");
                    }
                }
                //set wait command
                else if (wait.IsMatch(command))
                {
                    MainWindow.Log("MainWindow - RunPress - Found wait command, " + command);
                    try
                    {
                        int value = int.Parse(command.Split('(', ')')[1]);
                        MainWindow.Log("MainWindow - RunPress - wait time " + value.ToString());
                        Manager.AddCmd("wait", value);
                    }
                    catch
                    {
                        MainWindow.Log("MainWindow - RunPress - invalid wait time");
                    }
                }
                //invalid command
                else
                {
                    MainWindow.Log("MainWindow - RunPress - command invalid, " + command);
                }
            }
        }

        //stop script press
        private void StopPress(object sender, RoutedEventArgs e)
        {
            OAR.Stop();
        }

        //save script press
        private void SavePress(object sender, RoutedEventArgs e)
        {
            string filename;
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "";
            dlg.DefaultExt = ".xaml";
            dlg.Filter = "Windows Markup File |*.xaml";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                filename = dlg.FileName;
                TextRange range;
                FileStream fStream;
                range = new TextRange(txtScript.Document.ContentStart, txtScript.Document.ContentEnd);
                fStream = new FileStream(filename, FileMode.Create);
                range.Save(fStream, DataFormats.XamlPackage);
                fStream.Close();
                txtScriptPath.Text = filename;
            }
        }

        //close script press
        private void ClosePress(object sender, RoutedEventArgs e)
        {
            txtScript.Document.Blocks.Clear();
            txtScriptPath.Clear();
        }
        
        //help button press
        private void HelpPress(object sender, RoutedEventArgs e)
        {
            //TODO: create help dialog
        }

        //about button press
        private void AboutPress(object sender, RoutedEventArgs e)
        {
            //TODO: create about dialog
        }

    }
}