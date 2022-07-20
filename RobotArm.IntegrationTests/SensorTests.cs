using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RobotArm.IntegrationTests
{
    [TestClass]
    public class SensorTests
    {

        [TestMethod]
        public void Sensor_EvalProximity_QueueLastEqualsReadProximity()
        {
            //Arrange
            string id = "proximity";
            int limit = 12;
            bool objAvoid = true;
            var wndnw = new MainWindow();
            var sensor = new Sensor(id, limit, objAvoid);


            //Act
            sensor.EvalProximity();
            string value = MainWindow.Manager.QueueLast;

            //Assert
            Assert.AreEqual(value, "RobotArm.ReadProximity");
        }


    }
}
