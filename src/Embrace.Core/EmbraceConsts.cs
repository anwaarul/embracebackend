using Embrace.Debugging;

namespace Embrace
{
    public class EmbraceConsts
    {
        public const string LocalizationSourceName = "Embrace";

        public const string ConnectionStringName = "Default";

        public const bool MultiTenancyEnabled = true;


        /// <summary>
        /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
        /// </summary>
        public static readonly string DefaultPassPhrase =
            DebugHelper.IsDebug ? "gsKxGZ012HLL3MI5" : "c2e745315d874474b63bc4747a9b0848";
    }
}
