using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exceptions
{
    public class UnderflowException : OverflowException
    {
        public UnderflowException(string message) : base(message) { }
    }
}
