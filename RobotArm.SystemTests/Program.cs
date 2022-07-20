using Ranorex;
using System;
using System.Diagnostics;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RobotArm.SystemTests
{
    class Program
    {
        static void Main(string[] args)
        {
            InitResolver();
            RanorexInit();
            Run();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void InitResolver()
        {
            Ranorex.Core.Resolver.AssemblyLoader.Initialize();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void RanorexInit()
        {
            TestingBootstrapper.SetupCore();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static int Run()
        {
            int error = 0;
            //Start calculator and wait for UI to be loaded
            try
            {
                Process pr = Process.Start("C:\\Program Files (x86)\\Bouchard\\OAR\\RobotArm.exe");
                Thread.Sleep(3000);

                string processName = "RobotArm";
                
                WindowsApp oar = Host.Local.FindSingle<WindowsApp>("winapp[@processname='" + processName + "']");

                Button button = oar.FindSingle<Ranorex.Button>(".//button[@automationid='num2Button']");
                button.Click();

                pr.CloseMainWindow();
                pr.Close();


                //Get process name
                //string processName = GetActualCalculatorProcessName();

                //WindowsApp oar = Host.Local.FindSingle<WindowsApp>("winapp[@processname='" + processName + "']");

                //Thread.Sleep(5000);

                //Button button = calculator.FindSingle<Ranorex.Button>(".//button[@automationid='num2Button']");
                //button.Click();

                //button = calculator.FindSingle<Ranorex.Button>(".//button[@automationid='plusButton']");
                //button.Click();

                //button = calculator.FindSingle<Ranorex.Button>(".//button[@automationid='num3Button']");
                //button.Click();

                //button = calculator.FindSingle<Ranorex.Button>(".//button[@automationid='equalButton']");
                //button.Click();

                //Close calculator
                //oar.As<Form>().Close();
            }
            catch (RanorexException e)
            {
                Console.WriteLine(e.ToString());
                error = -1;
            }

            return error;
        }
        //private static string GetActualCalculatorProcessName()
        //{
        //    string processName = String.Empty;
        //    var processes = System.Diagnostics.Process.GetProcesses();

        //    foreach (var item in processes)
        //    {
        //        if (item.ProcessName.ToLowerInvariant().Contains("RobotArm"))
        //        {
        //            processName = item.ProcessName;
        //            break;
        //        }
        //    }

        //    return processName;
        //}
    }
}
