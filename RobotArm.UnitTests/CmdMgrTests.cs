using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RobotArm.UnitTests
{
    [TestClass]
    public class CmdMgrTests
    {
        [TestMethod]
        public void AddCmd_CmdEqualsStop_ReturnTrue()
        {
            //Arrange
            var wndnw = new MainWindow();
            var manager = new CmdMgr();
            string command = "stop";

            //Act
            var result = manager.AddCmd(command);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void AddCmd_CmdEqualsProximity_ReturnTrue()
        {
            //Arrange
            var wndnw = new MainWindow();
            var manager = new CmdMgr();
            string command = "proximity";

            //Act
            var result = manager.AddCmd(command);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void AddCmd_CmdEqualsBase_ReturnTrue()
        {
            //Arrange
            var wndnw = new MainWindow();
            var manager = new CmdMgr();
            string command = "base";
            int value = 16;

            //Act
            var result = manager.AddCmd(command, value);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void AddCmd_CmdEqualsShoulder_ReturnTrue()
        {
            //Arrange
            var wndnw = new MainWindow();
            var manager = new CmdMgr();
            string command = "shoulder";
            int value = 16;

            //Act
            var result = manager.AddCmd(command, value);

            //Assert
            Assert.IsTrue(result);
        }
        
        [TestMethod]
        public void AddCmd_CmdEqualsElbow_ReturnTrue()
        {
            //Arrange
            var wndnw = new MainWindow();
            var manager = new CmdMgr();
            string command = "elbow";
            int value = 16;

            //Act
            var result = manager.AddCmd(command, value);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void AddCmd_CmdEqualsGripper_ReturnTrue()
        {
            //Arrange
            var wndnw = new MainWindow();
            var manager = new CmdMgr();
            string command = "gripper";
            int value = 16;

            //Act
            var result = manager.AddCmd(command, value);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void AddCmd_CmdEqualsWait_ReturnTrue()
        {
            //Arrange
            var wndnw = new MainWindow();
            var manager = new CmdMgr();
            string command = "wait";
            int value = 3;

            //Act
            var result = manager.AddCmd(command, value);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void AddCmd_CmdEqualsInvalid_ReturnFalse()
        {
            //Arrange
            var wndnw = new MainWindow();
            var manager = new CmdMgr();
            string command = "invalid";

            //Act
            var result = manager.AddCmd(command);

            //Assert
            Assert.IsFalse(result);
        }
    }
}
