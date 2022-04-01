using Abp.Authorization;
using Embrace.Authorization.Roles;
using Embrace.Authorization.Users;

namespace Embrace.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
