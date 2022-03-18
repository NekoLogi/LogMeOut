using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogMeOut
{
    internal class Timer
    {
        public enum Mode
        {
            Shutdown,
            Restart,
            Cancel
        }


        public static string RunTimer(int days, int hours, int minutes, Mode mode)
        {
            switch (mode)
            {
                case Mode.Shutdown:
                    return ShutdownPC(days, hours, minutes);
                case Mode.Restart:
                    return RestartPC(days, hours, minutes);
                case Mode.Cancel:
                    return CancelTimer();
            }
            return null;
        }


        private static string ShutdownPC(int days, int hours, int minutes)
        {
            int seconds = GetSeconds(days, hours, minutes);
            RunCommand("/s /f /t", seconds);
            return ReturnMessage(days, hours, minutes, Mode.Shutdown);
        }

        private static string CancelTimer()
        {
            var process = new System.Diagnostics.ProcessStartInfo("shutdown", "/a")
            {
                CreateNoWindow = true,
                UseShellExecute = false
            };
            System.Diagnostics.Process.Start(process);

            return "Timer has been cancelled.";
        }

        private static string RestartPC(int days, int hours, int minutes)
        {
            int seconds = GetSeconds(days, hours, minutes);
            RunCommand("/r /f /t", seconds);
            return ReturnMessage(days, hours, minutes, Mode.Restart);
        }

        private static string ReturnMessage(int days, int hours, int minutes, Mode mode)
        {
            return $"PC will {mode} in {days} days {hours} hours {minutes} minutes.\n" +
                $"Expected Shutdown: {MainWindow.SHUTDOWN_DATE}";
        }

        private static int GetSeconds(int days, int hours, int minutes)
        {
            int seconds = 0;
            for (int i = 0; i < days; i++)
                seconds += 86400;
            for (int i = 0; i < hours; i++)
                seconds += 3600;
            for (int i = 0; i < minutes; i++)
                seconds += 60;

            return seconds;
        }

        private static void RunCommand(string mode, int seconds)
        {
            //System.Diagnostics.Process process1 = new System.Diagnostics.Process();
            //System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo
            //{
            //    FileName = "cmd.exe",
            //    Arguments = $"{command}",
            //    CreateNoWindow = false,
            //    RedirectStandardInput = true,
            //    RedirectStandardOutput = true,
            //};
            //process1.StartInfo = startInfo;
            //process1.Start();

            var process = new System.Diagnostics.ProcessStartInfo("shutdown", $"{mode} {seconds}")
            {
                CreateNoWindow = true,
                UseShellExecute = false
            };
            System.Diagnostics.Process.Start(process);
        }
    }
}
