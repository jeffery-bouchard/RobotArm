using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RobotArm
{
    //Window with drop-down menu for selecting a serial port
    public partial class SelectPort : Window
    {
        //list of available serial ports
        public string Port { get; set; }

        //Initialize serial port window
        public SelectPort()
        {
            InitializeComponent();
            MainWindow.Log("SelectPort - SelectPort - Initializing port select window");
        }

        //Method for capturing a selected serial port
        private void SelectPress(object sender, RoutedEventArgs e)
        {
            MainWindow.Log("SelectPort - SelectPress - Select port clicked");
            //no port selected
            if (boxPortName.SelectedIndex == -1)
            {
                MainWindow.Log("SelectPort - SelectPress - No port selected");
                txtPortStatus.Text = "Please select a port";
            }
            //port selected
            else
            {
                Port = boxPortName.Text;
                MainWindow.Log("SelectPort - SelectPress - Port Selected: " + Port);
                DialogResult = true;
                Close();
            }
                
        }
    }
}
