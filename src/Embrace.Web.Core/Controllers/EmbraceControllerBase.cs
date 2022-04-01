using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace Embrace.Controllers
{
    public abstract class EmbraceControllerBase: AbpController
    {
        protected EmbraceControllerBase()
        {
            LocalizationSourceName = EmbraceConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
