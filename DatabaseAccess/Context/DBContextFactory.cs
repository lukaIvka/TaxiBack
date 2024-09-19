using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.Context
{
    public sealed class DBContextFactory
    {
        private static readonly DBContextFactory instance = new DBContextFactory();
        private DBContextFactory() { }
        public static DBContextFactory Instance { get { return instance; } }

        private static DbContextOptionsBuilder OptionsBuilder { get; set; }

        public void InitDb(DbContextOptionsBuilder optionsBuilder)
        {
            OptionsBuilder = optionsBuilder;
            using var dbCtx = new TaxiDBContext(OptionsBuilder.Options);
            dbCtx.Database.EnsureCreated();
        }

        public TaxiDBContext GetDBContext()
        {
            return new TaxiDBContext(OptionsBuilder.Options);
        }
    }
}
