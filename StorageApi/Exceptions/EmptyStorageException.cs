using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorageApi.Exceptions
{
    public class EmptyStorageException : Exception
    {
        public EmptyStorageException(string errorMessage) : base(message: errorMessage) { }
    }
}
