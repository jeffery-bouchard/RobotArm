using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RobotArm.UnitTests
{
    [TestClass]
    public class ActuatorTests
    {
        [TestMethod]
        public void Increment_PositionIsLessThanMax_ReturnTrue()
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
            var result = actuator.Increment();

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Increment_PositionEqualsMax_ReturnFalse()
        {
            //Arrange
            string id = "gripper";
            int home = 16;
            int min = 0;
            int max = 31;
            int position = 31;
            bool inverted = false;
            var wndnw = new MainWindow();
            var actuator = new Actuator(id, home, min, max, position, inverted);

            //Act
            var result = actuator.Increment();

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Increment_InvertedPositionIsGreaterThanMin_ReturnTrue()
        {
            //Arrange
            string id = "gripper";
            int home = 16;
            int min = 0;
            int max = 31;
            int position = 1;
            bool inverted = true;
            var wndnw = new MainWindow();
            var actuator = new Actuator(id, home, min, max, position, inverted);

            //Act
            var result = actuator.Increment();

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Increment_InvertedPositionEqualsMin_ReturnFalse()
        {
            //Arrange
            string id = "gripper";
            int home = 16;
            int min = 0;
            int max = 31;
            int position = 0;
            bool inverted = true;
            var wndnw = new MainWindow();
            var actuator = new Actuator(id, home, min, max, position, inverted);

            //Act
            var result = actuator.Increment();

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Decrement_PositionGreaterThanMin_ReturnTrue()
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
            var result = actuator.Decrement();

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Decrement_PositionEqualsMin_ReturnFalse()
        {
            //Arrange
            string id = "gripper";
            int home = 16;
            int min = 0;
            int max = 31;
            int position = 0;
            bool inverted = false;
            var wndnw = new MainWindow();
            var actuator = new Actuator(id, home, min, max, position, inverted);

            //Act
            var result = actuator.Decrement();

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Decrement_InvertedPositionLessThanMax_ReturnTrue()
        {
            //Arrange
            string id = "gripper";
            int home = 16;
            int min = 0;
            int max = 31;
            int position = 30;
            bool inverted = true;
            var wndnw = new MainWindow();
            var actuator = new Actuator(id, home, min, max, position, inverted);

            //Act
            var result = actuator.Decrement();

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Decrement_InvertedPositionEqualsMax_ReturnFalse()
        {
            //Arrange
            string id = "gripper";
            int home = 16;
            int min = 0;
            int max = 31;
            int position = 31;
            bool inverted = true;
            var wndnw = new MainWindow();
            var actuator = new Actuator(id, home, min, max, position, inverted);

            //Act
            var result = actuator.Decrement();

            //Assert
            Assert.IsFalse(result);
        }
    }
}
