using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace RaspberryPi.GPIO.I2C
{
    internal static class I2CNativeLib
    {
        [DllImport("libnativei2c.so", EntryPoint = "openBus", SetLastError = true)]
        public static extern int OpenBus(string busFileName);

        [DllImport("libnativei2c.so", EntryPoint = "closeBus", SetLastError = true)]
        public static extern int CloseBus(int busHandle);

        [DllImport("libnativei2c.so", EntryPoint = "readBytes", SetLastError = true)]
        public static extern int ReadBytes(int busHandle, int addr, byte[] buf, int len, int debug);

        [DllImport("libnativei2c.so", EntryPoint = "writeBytes", SetLastError = true)]
        public static extern int WriteBytes(int busHandle, int addr, byte[] buf, int len, int debug);
    }
}
