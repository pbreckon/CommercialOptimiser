using System;
using Microsoft.EntityFrameworkCore;

namespace CommercialOptimiser.Data.SqlServer
{
    public class DatabaseInitializerSqlServer
    {
        public DbContextOptionsBuilder GetWhatever(
            DbContextOptionsBuilder options,
            string connectionString)
        {
            return options.UseSqlServer(connectionString);
        }
    }
}
