using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using Embrace.Authorization.Roles;
using Embrace.Authorization.Users;
using Embrace.MultiTenancy;

namespace Embrace.EntityFrameworkCore
{
    public class EmbraceDbContext : AbpZeroDbContext<Tenant, Role, User, EmbraceDbContext>
    {
        /* Define a DbSet for each entity of the application */
        
        public EmbraceDbContext(DbContextOptions<EmbraceDbContext> options)
            : base(options)
        {
        }
    }
}
