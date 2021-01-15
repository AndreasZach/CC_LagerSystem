using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorageApi.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string errorMessage) : base(message: errorMessage) { }
    }
}
