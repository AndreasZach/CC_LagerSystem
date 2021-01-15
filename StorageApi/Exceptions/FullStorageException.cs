using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorageApi.Exceptions
{
    public class FullStorageException : Exception
    {
        public FullStorageException(string errorMessage) : base(message: errorMessage) { }
    }
}
