using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RobotArm.IntegrationTests
{
    [TestClass]
    public class MotorTests
    {
        [TestMethod]
        public void Actuator_Increment_QueueLastEqualsSetActuator()
        {
            //Arrange
            string id = "gripper";
            int home = 16;
            int min = 0;
            int max = 31;
            int position = 30;
            bool inverted = false;
            var wndnw = new MainWindow();
            var actuator = new Actuator(id, home, min, max, position, inverted);

            //Act
            actuator.Increment();
            string value = MainWindow.Manager.QueueLast;

            //Assert
            Assert.AreEqual(value, "RobotArm.SetActuator");
        }

        [TestMethod]
        public void Actuator_Decrement_QueueLastEqualsSetActuator()
        {
            //Arrange
            string id = "gripper";
            int home = 16;
            int min = 0;
            int max = 31;
            int position = 1;
            bool inverted = false;
            var wndnw = new MainWindow();
            var actuator = new Actuator(id, home, min, max, position, inverted);

            //Act
            actuator.Decrement();
            string value = MainWindow.Manager.QueueLast;

            //Assert
            Assert.AreEqual(value, "RobotArm.SetActuator");
        }

        [TestMethod]
        public void Actuator_GoHome_QueueLastEqualsSetActuator()
        {
            //Arrange
            string id = "gripper";
            int home = 16;
            int min = 0;
            int max = 31;
            int position = 1;
            bool inverted = false;
            var wndnw = new MainWindow();
            var actuator = new Actuator(id, home, min, max, position, inverted);

            //Act
            actuator.GoHome();
            string value = MainWindow.Manager.QueueLast;

            //Assert
            Assert.AreEqual(value, "RobotArm.SetActuator");
        }

        [TestMethod]
        public void Actuator_Stop_QueueLastEqualsStop()
        {
            //Arrange
            string id = "gripper";
            int home = 16;
            int min = 0;
            int max = 31;
            int position = 1;
            bool inverted = false;
            var wndnw = new MainWindow();
            var actuator = new Actuator(id, home, min, max, position, inverted);

            //Act
            actuator.Stop();
            string value = MainWindow.Manager.QueueLast;

            //Assert
            Assert.AreEqual(value, "RobotArm.Stop");
        }

        [TestMethod]
        public void Robot_Stop_QueueLastEqualsStop()
        {
            //Arrange
            var wndnw = new MainWindow();
            var robot = new Robot();

            //Act
            robot.Stop();
            string value = MainWindow.Manager.QueueLast;

            //Assert
            Assert.AreEqual(value, "RobotArm.Stop");
        }

        [TestMethod]
        public void Robot_GoHome_QueueLastEqualsSetActuator()
        {
            //Arrange
            var wndnw = new MainWindow();
            var robot = new Robot();

            //Act
            robot.GoHome();
            string value = MainWindow.Manager.QueueLast;

            //Assert
            Assert.AreEqual(value, "RobotArm.SetActuator");
        }
    }
}
