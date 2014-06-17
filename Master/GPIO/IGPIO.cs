using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaspberryPi.GPIO
{
    public interface IGPIO : IDisposable
    {
        bool Read();
    }
}
