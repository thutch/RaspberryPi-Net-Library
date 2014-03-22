using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaspberryPi.GPIO.I2C.ADC
{
    public class ADS1x15Exception : Exception
    {
        public ADS1x15Exception()
        {

        }

        public ADS1x15Exception(string message)
            : base(message)
        {

        }

        public ADS1x15Exception(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
