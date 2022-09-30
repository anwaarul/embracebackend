using Xunit;

namespace Embrace.Tests
{
    public sealed class MultiTenantFactAttribute : FactAttribute
    {
        public MultiTenantFactAttribute()
        {
            if (!EmbraceConsts.MultiTenancyEnabled)
            {
#pragma warning disable CS0162 // Unreachable code detected
                Skip = "MultiTenancy is disabled.";
#pragma warning restore CS0162 // Unreachable code detected
            }
        }
    }
}
