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
            //SubscriptionType Permission

            context.CreatePermission(PermissionNames.LookUps_SubscriptionType, L("SubscriptionType"));
            context.CreatePermission(PermissionNames.LookUps_SubscriptionType_Page, L("PageSubscriptionType"));
            context.CreatePermission(PermissionNames.LookUps_SubscriptionType_Create, L("CreateSubscriptionType"));
            context.CreatePermission(PermissionNames.LookUps_SubscriptionType_Update, L("UpdateSubscriptionType"));
            context.CreatePermission(PermissionNames.LookUps_SubscriptionType_Delete, L("DeleteSubscriptionType"));
            //SubscriptionOrderPayementAllocation Permission

            context.CreatePermission(PermissionNames.LookUps_SubscriptionOrderPayementAllocation, L("SubscriptionOrderPayementAllocation"));
            context.CreatePermission(PermissionNames.LookUps_SubscriptionOrderPayementAllocation_Page, L("PageSubscriptionOrderPayementAllocation"));
            context.CreatePermission(PermissionNames.LookUps_SubscriptionOrderPayementAllocation_Create, L("CreateSubscriptionOrderPayementAllocation"));
            context.CreatePermission(PermissionNames.LookUps_SubscriptionOrderPayementAllocation_Update, L("UpdateSubscriptionOrderPayementAllocation"));
            context.CreatePermission(PermissionNames.LookUps_SubscriptionOrderPayementAllocation_Delete, L("DeleteSubscriptionOrderPayementAllocation"));
            //Subscription Permission

            context.CreatePermission(PermissionNames.LookUps_Subscription, L("Subscription"));
            context.CreatePermission(PermissionNames.LookUps_Subscription_Page, L("PageSubscription"));
            context.CreatePermission(PermissionNames.LookUps_Subscription_Create, L("CreateSubscription"));
            context.CreatePermission(PermissionNames.LookUps_Subscription_Update, L("UpdateSubscription"));
            context.CreatePermission(PermissionNames.LookUps_Subscription_Delete, L("DeleteSubscription"));

            //OrderPlacement Permission

            context.CreatePermission(PermissionNames.LookUps_OrderPlacement, L("OrderPlacement"));
            context.CreatePermission(PermissionNames.LookUps_OrderPlacement_Page, L("PageOrderPlacement"));
            context.CreatePermission(PermissionNames.LookUps_OrderPlacement_Create, L("CreateOrderPlacement"));
            context.CreatePermission(PermissionNames.LookUps_OrderPlacement_Update, L("UpdateOrderPlacement"));
            context.CreatePermission(PermissionNames.LookUps_OrderPlacement_Delete, L("DeleteOrderPlacement"));
            //ProductCategory Permission

            context.CreatePermission(PermissionNames.LookUps_ProductCategory, L("ProductCategory"));
            context.CreatePermission(PermissionNames.LookUps_ProductCategory_Page, L("PageProductCategory"));
            context.CreatePermission(PermissionNames.LookUps_ProductCategory_Create, L("CreateProductCategory"));
            context.CreatePermission(PermissionNames.LookUps_ProductCategory_Update, L("UpdateProductCategory"));
            context.CreatePermission(PermissionNames.LookUps_ProductCategory_Delete, L("DeleteProductCategory"));
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

            //ProductType Permission
            context.CreatePermission(PermissionNames.LookUps_ProductType, L("ProductType"));
            context.CreatePermission(PermissionNames.LookUps_ProductType_Page, L("PageProductType"));
            context.CreatePermission(PermissionNames.LookUps_ProductType_Create, L("CreateProductType"));
            context.CreatePermission(PermissionNames.LookUps_ProductType_Update, L("UpdateProductType"));
            context.CreatePermission(PermissionNames.LookUps_ProductType_Delete, L("DeleteProductType"));

            //ProductParameterVariantAllocation Permission
            context.CreatePermission(PermissionNames.LookUps_ProductParameterVariantAllocation, L("ProductParameterVariantAllocation"));
            context.CreatePermission(PermissionNames.LookUps_ProductParameterVariantAllocation_Page, L("PageProductParameterVariantAllocation"));
            context.CreatePermission(PermissionNames.LookUps_ProductParameterVariantAllocation_Create, L("CreateProductParameterVariantAllocation"));
            context.CreatePermission(PermissionNames.LookUps_ProductParameterVariantAllocation_Update, L("UpdateProductParameterVariantAllocation"));
            context.CreatePermission(PermissionNames.LookUps_ProductParameterVariantAllocation_Delete, L("DeleteProductParameterVariantAllocation"));

            //ProductParameterSizeAllocation Permission
            context.CreatePermission(PermissionNames.LookUps_ProductParameterSizeAllocation, L("ProductParameterSizeAllocation"));
            context.CreatePermission(PermissionNames.LookUps_ProductParameterSizeAllocation_Page, L("PageProductParameterSizeAllocation"));
            context.CreatePermission(PermissionNames.LookUps_ProductParameterSizeAllocation_Create, L("CreateProductParameterSizeAllocation"));
            context.CreatePermission(PermissionNames.LookUps_ProductParameterSizeAllocation_Update, L("UpdateProductParameterSizeAllocation"));
            context.CreatePermission(PermissionNames.LookUps_ProductParameterSizeAllocation_Delete, L("DeleteProductParameterSizeAllocation"));

            //UserProductAllocation Permission
            context.CreatePermission(PermissionNames.LookUps_UserProductAllocation, L("UserProductAllocation"));
            context.CreatePermission(PermissionNames.LookUps_UserProductAllocation_Page, L("PageUserProductAllocation"));
            context.CreatePermission(PermissionNames.LookUps_UserProductAllocation_Create, L("CreateUserProductAllocation"));
            context.CreatePermission(PermissionNames.LookUps_UserProductAllocation_Update, L("UpdateUserProductAllocation"));
            context.CreatePermission(PermissionNames.LookUps_UserProductAllocation_Delete, L("DeleteUserProductAllocation"));

            //ProductImageAllocation Permission
            context.CreatePermission(PermissionNames.LookUps_ProductImageAllocation, L("ProductImageAllocation"));
            context.CreatePermission(PermissionNames.LookUps_ProductImageAllocation_Page, L("PageProductImageAllocation"));
            context.CreatePermission(PermissionNames.LookUps_ProductImageAllocation_Create, L("CreateProductImageAllocation"));
            context.CreatePermission(PermissionNames.LookUps_ProductImageAllocation_Update, L("UpdateProductImageAllocation"));
            context.CreatePermission(PermissionNames.LookUps_ProductImageAllocation_Delete, L("DeleteProductImageAllocation"));

            //Blog Permission
            context.CreatePermission(PermissionNames.LookUps_Blog, L("Blog"));
            context.CreatePermission(PermissionNames.LookUps_Blog_Page, L("PageBlog"));
            context.CreatePermission(PermissionNames.LookUps_Blog_Create, L("CreateBlog"));
            context.CreatePermission(PermissionNames.LookUps_Blog_Update, L("UpdateBlog"));
            context.CreatePermission(PermissionNames.LookUps_Blog_Delete, L("DeleteBlog"));

            //BlogCategory Permission
            context.CreatePermission(PermissionNames.LookUps_BlogCategory, L("BlogCategory"));
            context.CreatePermission(PermissionNames.LookUps_BlogCategory_Page, L("PageBlogCategory"));
            context.CreatePermission(PermissionNames.LookUps_BlogCategory_Create, L("CreateBlogCategory"));
            context.CreatePermission(PermissionNames.LookUps_BlogCategory_Update, L("UpdateBlogCategory"));
            context.CreatePermission(PermissionNames.LookUps_BlogCategory_Delete, L("DeleteBlogCategory"));

            //BlogBlogCategoryAllocation Permission
            context.CreatePermission(PermissionNames.LookUps_BlogBlogCategoryAllocation, L("BlogBlogCategoryAllocation"));
            context.CreatePermission(PermissionNames.LookUps_BlogBlogCategoryAllocation_Page, L("PageBlogBlogCategoryAllocation"));
            context.CreatePermission(PermissionNames.LookUps_BlogBlogCategoryAllocation_Create, L("CreateBlogBlogCategoryAllocation"));
            context.CreatePermission(PermissionNames.LookUps_BlogBlogCategoryAllocation_Update, L("UpdateBlogBlogCategoryAllocation"));
            context.CreatePermission(PermissionNames.LookUps_BlogBlogCategoryAllocation_Delete, L("DeleteBlogBlogCategoryAllocation"));

            //Alert Permission
            context.CreatePermission(PermissionNames.LookUps_Alert, L("Alert"));
            context.CreatePermission(PermissionNames.LookUps_Alert_Page, L("PageAlert"));
            context.CreatePermission(PermissionNames.LookUps_Alert_Create, L("CreateAlert"));
            context.CreatePermission(PermissionNames.LookUps_Alert_Update, L("UpdateAlert"));
            context.CreatePermission(PermissionNames.LookUps_Alert_Delete, L("DeleteAlert"));

        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, EmbraceConsts.LocalizationSourceName);
        }
    }
}
