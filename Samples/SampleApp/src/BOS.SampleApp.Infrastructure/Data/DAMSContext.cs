using BOS.SampleApp.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BOS.SampleApp.Infrastructure.Data
{
    public class DAMSContext : DbContext
    {
        public DAMSContext(DbContextOptions<DAMSContext> options)
            : base(options)
        {
        }

        public DbSet<CollectionEntity> Collections { get; set; }
    }
}
