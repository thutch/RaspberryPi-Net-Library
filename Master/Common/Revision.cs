using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace RaspberryPi.Common
{
    class Revision
    {
        private static List<string> revBetaList = new List<string>() { "beta" };
        private static List<string> rev1List = new List<string>() { "0002", "0003" };
        private static List<string> rev2List = new List<string>() { "0004", "0005", "0006", "0007", "0008", "0009", "000d", "000e", "000f" };

        public static int Get()
        {
            string rpiRawVersion = string.Empty;

            ProcessStartInfo psi = new ProcessStartInfo();
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.RedirectStandardOutput = true;
            psi.FileName = "bash";
            psi.Arguments = "-c \"cat /proc/cpuinfo | grep Revision\"";

            using (Process proc = new Process())
            {
                proc.StartInfo = psi;
                proc.Start();

                while (!proc.StandardOutput.EndOfStream)
                {
                    rpiRawVersion = proc.StandardOutput.ReadLine().Trim().ToLower();
                    rpiRawVersion = rpiRawVersion.Substring(rpiRawVersion.Length - 4, 4);
                }
            }

            if (revBetaList.FindAll(x => x == rpiRawVersion).Count != 0)
                return 0;
            else if (rev1List.FindAll(x => x == rpiRawVersion).Count != 0)
                return 1;
            else if (rev2List.FindAll(x => x == rpiRawVersion).Count != 0)
                return 2;
            else
                throw new RevisionException(String.Format("Unknown Raspberry Pi revision detected. [{0}]", rpiRawVersion));
        }
    }
}
