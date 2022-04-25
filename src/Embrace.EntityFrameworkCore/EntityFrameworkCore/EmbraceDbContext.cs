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
        public DbSet<CategoryInfo> CategoryInfo { get; set; }
        public DbSet<SubCategoryImageAllocationInfo> SubCategoryImageAllocationInfo { get; set; }
        public DbSet<SubCategoryInfo> SubCategoryInfo { get; set; }
        public DbSet<UsersDetailsInfo> UsersDetailsInfo { get; set; }
        public DbSet<UniqueNameAndDateInfo> UniqueNameAndDateInfo { get; set; }
        public DbSet<MenstruationDetailsInfo> MenstruationDetailsInfo { get; set; }
        public DbSet<SubCategoryAndDateInfo> SubCategoryAndDateInfo { get; set; }
        public DbSet<ProductCategoriesInfo> ProductCategoriesInfo { get; set; }
        public DbSet<ProductVariantsInfo> ProductVariantsInfo { get; set; }
        public DbSet<SizeInfo> SizeInfo { get; set; }
        public DbSet<ProductParametersInfo> ProductParametersInfo { get; set; }
        public DbSet<UserProductAllocationInfo> UserProductAllocationInfo { get; set; }
        public DbSet<ProductTypeInfo> ProductTypeInfo { get; set; }
        public DbSet<ProductParameterSizeAllocationInfo> ProductParameterSizeAllocationInfo { get; set; }
        public DbSet<ProductParameterVariantAllocationInfo> ProductParameterVariantAllocationInfo { get; set; }
        public DbSet<ProductImageAllocationInfo> ProductImageAllocationInfo { get; set; }

    }
}
