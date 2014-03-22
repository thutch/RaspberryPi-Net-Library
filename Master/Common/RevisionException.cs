using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaspberryPi.Common
{
    public class RevisionException : Exception
    {
        public RevisionException()
        {

        }

        public RevisionException(string message)
            : base(message)
        {

        }

        public RevisionException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
