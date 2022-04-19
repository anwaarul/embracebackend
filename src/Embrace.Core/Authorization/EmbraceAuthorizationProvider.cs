using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace Embrace.Authorization
{
    public class EmbraceAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            context.CreatePermission(PermissionNames.Pages_Users, L("Users"));
            context.CreatePermission(PermissionNames.Pages_Users_Activation, L("UsersActivation"));
            context.CreatePermission(PermissionNames.Pages_Roles, L("Roles"));
            context.CreatePermission(PermissionNames.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
            
            //Category Permission

            context.CreatePermission(PermissionNames.LookUps_Category, L("Category"));
            context.CreatePermission(PermissionNames.LookUps_Category_Page, L("PageCategory"));
            context.CreatePermission(PermissionNames.LookUps_Category_Create, L("CreateCategory"));
            context.CreatePermission(PermissionNames.LookUps_Category_Update, L("UpdateCategory"));
            context.CreatePermission(PermissionNames.LookUps_Category_Delete, L("DeleteCategory"));
            //CategorySubCategoryAllocation Permission

            context.CreatePermission(PermissionNames.LookUps_CategorySubCategoryAllocation, L("CategorySubCategoryAllocation"));
            context.CreatePermission(PermissionNames.LookUps_CategorySubCategoryAllocation_Page, L("PageCategorySubCategoryAllocation"));
            context.CreatePermission(PermissionNames.LookUps_CategorySubCategoryAllocation_Create, L("CreateCategorySubCategoryAllocation"));
            context.CreatePermission(PermissionNames.LookUps_CategorySubCategoryAllocation_Update, L("UpdateCategorySubCategoryAllocation"));
            context.CreatePermission(PermissionNames.LookUps_CategorySubCategoryAllocation_Delete, L("DeleteCategorySubCategoryAllocation"));
            //SubCategoryImageAllocation Permission

            context.CreatePermission(PermissionNames.LookUps_SubCategoryImageAllocation, L("SubCategoryImageAllocation"));
            context.CreatePermission(PermissionNames.LookUps_SubCategoryImageAllocation_Page, L("PageSubCategoryImageAllocation"));
            context.CreatePermission(PermissionNames.LookUps_SubCategoryImageAllocation_Create, L("CreateSubCategoryImageAllocation"));
            context.CreatePermission(PermissionNames.LookUps_SubCategoryImageAllocation_Update, L("UpdateSubCategoryImageAllocation"));
            context.CreatePermission(PermissionNames.LookUps_SubCategoryImageAllocation_Delete, L("DeleteSubCategoryImageAllocation"));
            //SubCategory Permission

            context.CreatePermission(PermissionNames.LookUps_SubCategory, L("SubCategory"));
            context.CreatePermission(PermissionNames.LookUps_SubCategory_Page, L("PageSubCategory"));
            context.CreatePermission(PermissionNames.LookUps_SubCategory_Create, L("CreateSubCategory"));
            context.CreatePermission(PermissionNames.LookUps_SubCategory_Update, L("UpdateSubCategory"));
            context.CreatePermission(PermissionNames.LookUps_SubCategory_Delete, L("DeleteSubCategory"));
            
            //UsersDetails Permission
            context.CreatePermission(PermissionNames.LookUps_UsersDetails, L("UsersDetails"));
            context.CreatePermission(PermissionNames.LookUps_UsersDetails_Page, L("PageUsersDetails"));
            context.CreatePermission(PermissionNames.LookUps_UsersDetails_Create, L("CreateUsersDetails"));
            context.CreatePermission(PermissionNames.LookUps_UsersDetails_Update, L("UpdateUsersDetails"));
            context.CreatePermission(PermissionNames.LookUps_UsersDetails_Delete, L("DeleteUsersDetails"));

            //ProductCategories Permission
            context.CreatePermission(PermissionNames.LookUps_ProductCategories, L("ProductCategories"));
            context.CreatePermission(PermissionNames.LookUps_ProductCategories_Page, L("PageProductCategories"));
            context.CreatePermission(PermissionNames.LookUps_ProductCategories_Create, L("CreateProductCategories"));
            context.CreatePermission(PermissionNames.LookUps_ProductCategories_Update, L("UpdateProductCategories"));
            context.CreatePermission(PermissionNames.LookUps_ProductCategories_Delete, L("DeleteProductCategories"));

            //ProductVariants Permission
            context.CreatePermission(PermissionNames.LookUps_ProductVariants, L("ProductVariants"));
            context.CreatePermission(PermissionNames.LookUps_ProductVariants_Page, L("PageProductVariants"));
            context.CreatePermission(PermissionNames.LookUps_ProductVariants_Create, L("CreateProductVariants"));
            context.CreatePermission(PermissionNames.LookUps_ProductVariants_Update, L("UpdateProductVariants"));
            context.CreatePermission(PermissionNames.LookUps_ProductVariants_Delete, L("DeleteProductVariants"));

            //Size Permission
            context.CreatePermission(PermissionNames.LookUps_Size, L("Size"));
            context.CreatePermission(PermissionNames.LookUps_Size_Page, L("PageSize"));
            context.CreatePermission(PermissionNames.LookUps_Size_Create, L("CreateSize"));
            context.CreatePermission(PermissionNames.LookUps_Size_Update, L("UpdateSize"));
            context.CreatePermission(PermissionNames.LookUps_Size_Delete, L("DeleteSize"));

            //ProductParameters Permission
            context.CreatePermission(PermissionNames.LookUps_ProductParameters, L("ProductParameters"));
            context.CreatePermission(PermissionNames.LookUps_ProductParameters_Page, L("PageProductParameters"));
            context.CreatePermission(PermissionNames.LookUps_ProductParameters_Create, L("CreateProductParameters"));
            context.CreatePermission(PermissionNames.LookUps_ProductParameters_Update, L("UpdateProductParameters"));
            context.CreatePermission(PermissionNames.LookUps_ProductParameters_Delete, L("DeleteProductParameters"));

        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, EmbraceConsts.LocalizationSourceName);
        }
    }
}
