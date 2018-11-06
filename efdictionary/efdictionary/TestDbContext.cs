using efdictionary.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;

namespace efdictionary
{
    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions options)
       : base(options)
        {

        }

        public DbSet<Parent> Parents { get; set; }
        public DbSet<Child> Childs { get; set; }


    }
    public static class DbContextHelper
    {
        public static DbContextOptions<TDbContext> CreateDbContextOptions<TDbContext>()
            where TDbContext : DbContext
        {
            var options = new DbContextOptionsBuilder<TDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture))
                .Options;
            return options;
        }

    }
}
