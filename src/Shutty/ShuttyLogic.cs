using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shutty
{
    public class ShuttyLogic
    {
        internal static void ShutDown(bool force)
        {
            StringBuilder b = new StringBuilder();
            b.Append("/s /t 0");
            if (force)
            {
                b.Append(" /f");
            }
            b.Append(" /c Shutty");            
            StartProc(b.ToString());
        }

        internal static void ShutDown(bool force, int delay)
        {
            StringBuilder b = new StringBuilder();
            b.Append($"/s /t {delay}");
            if(force)
            {
                b.Append(" /f");
            }
            b.Append(" /c Shutty");
            StartProc(b.ToString());
        }

        internal static void Hibernate(bool force)
        {
            if (force)
                StartProc("/h /f");
            else
                StartProc("/h");
        }

        internal static void Reboot(bool force)
        {
            if (force)
                StartProc("/r /f");
            else
                StartProc("/r");
        }

        internal static void CancelShutdown()
        {
            StartProc("/a");
        }

        private static void StartProc(string args)
        {
            //args= "/s /t 0"
            var psi = new ProcessStartInfo("shutdown", args);
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            Process.Start(psi);
        }

    }

}
