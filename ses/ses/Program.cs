using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ses
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if(args.Length != 1)
            {
                Console.WriteLine("Insufficient number of arguments. Simply pass an integer representing the target process ID.");
                return;
            }

            int targetProcId = 0;

            if(!int.TryParse(args[0], out targetProcId))
            {
                Console.WriteLine("Invalid parameter. Simply pass an integer representing the target process ID.");
                return;
            }

            var theProcess = Process.GetProcessById(targetProcId);

            int accepted = 0, rejected = 0;
            foreach(var handle in WinAPI.EnumerateProcessWindowHandles(targetProcId))
            {
                if(WinAPI.SendMessage(handle, (uint)WM.QUERYENDSESSION, 0, 0))
                {
                    ++accepted;
                    WinAPI.SendMessage(handle, (uint)WM.ENDSESSION, true, 0);
                }
                else
                {
                    ++rejected;
                }
            }

            if(accepted > 0)
            {
                if(rejected > 0)
                {
                    Console.WriteLine("The process accepted the message about an impending session end {0} time(s), but also rejected it {1} time(s).", accepted, rejected);
                }
                else
                {
                    Console.WriteLine("The process accepted the message about an impending session end.");
                }

                Console.WriteLine("Giving the application 5 seconds to clean itself up, like Windows does during shutdown...");

                Task.Delay(TimeSpan.FromSeconds(5)).Wait();

                if(theProcess.HasExited)
                {
                    Console.WriteLine("Process appears to have correctly shut itself down in response to these messages.");
                }
                else
                {
                    Console.WriteLine("Process has failed to correctly shut itself down in response to these messages.");
                }
            }
            else
            {
                Console.Write("No WM_QUERYENDSESSION messages were accepted. This means that either the process has no windows, or the process entirely rejected the message about an impending session end.");
            }
        }
    }
}