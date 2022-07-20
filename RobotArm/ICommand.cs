using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotArm
{

    //Command object interface for command design pattern
    interface ICommand
    {
        int Execute();
    }
}
