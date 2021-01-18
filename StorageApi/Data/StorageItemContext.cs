using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StorageApi.Models;

namespace StorageApi.Data
{
    public class StorageItemContext : DbContext
    {
        public StorageItemContext (DbContextOptions<StorageItemContext> options)
            : base(options)
        {
        }

        public DbSet<StorageItem> StorageItems { get; set; }
    }
}
