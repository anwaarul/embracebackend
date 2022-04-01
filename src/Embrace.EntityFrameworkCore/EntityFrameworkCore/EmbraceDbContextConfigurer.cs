using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Embrace.EntityFrameworkCore
{
    public static class EmbraceDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<EmbraceDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<EmbraceDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
