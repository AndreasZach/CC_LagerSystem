using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StorageApi.Models;

namespace StorageApi.Interfaces
{
    public interface IDataStorage
    { 
        List<StorageItem> StoredItems { get; set; }
    }
}
