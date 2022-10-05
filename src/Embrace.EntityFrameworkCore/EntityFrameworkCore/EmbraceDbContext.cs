using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using Embrace.Authorization.Roles;
using Embrace.Authorization.Users;
using Embrace.MultiTenancy;
using Embrace.Entities;

namespace Embrace.EntityFrameworkCore
{
    public class EmbraceDbContext : AbpZeroDbContext<Tenant, Role, User, EmbraceDbContext>
    {
        /* Define a DbSet for each entity of the application */

        public EmbraceDbContext(DbContextOptions<EmbraceDbContext> options)
            : base(options)
        {
        }
        public DbSet<SubscriptionInfo> SubscriptionInfo { get; set; }
        public DbSet<SubscriptionOrderPayementAllocationInfo> SubscriptionOrderPayementAllocationInfo { get; set; }
        public DbSet<SubscriptionTypeInfo> SubscriptionTypeInfo { get; set; }
        public DbSet<SubCategoryImageAllocationInfo> SubCategoryImageAllocationInfo { get; set; }
        public DbSet<SubCategoryInfo> SubCategoryInfo { get; set; }
        public DbSet<UsersDetailsInfo> UsersDetailsInfo { get; set; }
        public DbSet<UniqueNameAndDateInfo> UniqueNameAndDateInfo { get; set; }
        public DbSet<MenstruationDetailsInfo> MenstruationDetailsInfo { get; set; }
        public DbSet<SubCategoryAndDateInfo> SubCategoryAndDateInfo { get; set; }
        public DbSet<ProductParametersInfo> ProductParametersInfo { get; set; }
        public DbSet<UserProductAllocationInfo> UserProductAllocationInfo { get; set; }
        public DbSet<ProductImageAllocationInfo> ProductImageAllocationInfo { get; set; }
        public DbSet<ProductCategoryInfo> ProductCategoryInfo { get; set; }
        public DbSet<OrderPlacementInfo> OrderPlacementInfo { get; set; }
        public DbSet<CategoryInfo> CategoryInfo { get; set; }
        public DbSet<BlogInfo> BlogInfo { get; set; }
        public DbSet<BlogCategoryInfo> BlogCategoryInfo { get; set; }
        public DbSet<BlogBlogCategoryAllocationInfo> BlogBlogCategoryAllocationInfo { get; set; }    
        public DbSet<UserBlogInfo> UserBlogInfo { get; set; }    

    }
}
