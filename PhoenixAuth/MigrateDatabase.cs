using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

namespace PhoenixAuth
{
    public static class MigrateDatabase
    {
        public static void InitializeDatabase<TContext>(this IApplicationBuilder app, TContext dbContext)
            where TContext : DbContext
        {

            dbContext.Database.Migrate();
            //payPointDbContext.Database.Migrate();

        }
    }
}
