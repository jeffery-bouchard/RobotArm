using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RobotArm.UnitTests
{
    [TestClass]
    public class MainWindowTests
    {
        [TestMethod]
        public void SetStatus_StatusIsEmpty_ReturnTrue()
        {
            //Arrange
            var mainWdw = new MainWindow();

            //Act
            var result = mainWdw.SetStatus("");

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SetStatus_StatusIs50Char_ReturnTrue()
        {
            //Arrange
            var mainWdw = new MainWindow();
            string FiftyChar = "12345678901234567890123456789012345678901234567890";

            //Act
            var result = mainWdw.SetStatus(FiftyChar);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SetStatus_StatusIs51Char_ReturnFalse()
        {
            //Arrange
            var mainWdw = new MainWindow();
            string FiftyOneChar = "123456789012345678901234567890123456789012345678901";

            //Act
            var result = mainWdw.SetStatus(FiftyOneChar);

            //Assert
            Assert.IsFalse(result);
        }
        
    }
}
