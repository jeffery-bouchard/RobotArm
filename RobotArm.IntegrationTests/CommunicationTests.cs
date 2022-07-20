using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace RobotArm.IntegrationTests
{
    [TestClass]
    public class CommunicationTests
    {
        [TestMethod]
        [TestCategory("Hardware Required")]
        public void CmdMgr_OnStart_QueueCntEquals0()
        {
            //Arrange
            var wndnw = new MainWindow();
            MainWindow.Comm.PortName = "COM3";
            MainWindow.Comm.BaudRate = 10417;
            MainWindow.Comm.Parity = Parity.None;
            MainWindow.Comm.DataBits = 8;
            MainWindow.Comm.StopBits = StopBits.One;
            MainWindow.Comm.Open();

            //Act
            MainWindow.Manager.Stop();
            MainWindow.Manager.StartQueue();
            Thread.Sleep(1000);
            int value = MainWindow.Manager.QueueCnt;
            MainWindow.Manager.StopQueue();

            //Assert
            Assert.AreEqual(value, 0);
        }
    }
}
