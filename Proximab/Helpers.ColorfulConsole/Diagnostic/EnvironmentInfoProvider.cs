using System;

namespace Helpers.ColorfulConsole.Diagnostic
{
    /// <summary>
    /// Represents a set of methods for retrieving OS information.
    /// </summary>
    internal class EnvironmentInfoProvider
    {
        /// <summary>
        /// Gets the full operating system version.
        /// </summary>
        public string OsInfo => Environment.OSVersion.VersionString;

        /// <summary>
        /// Gets the platform version (32/64 bits).
        /// </summary>
        public string CpuPlatformVersion => Environment.Is64BitProcess ? "64bit" : "32bit";

        /// <summary>
        /// Gets the process version (32/64 bits).
        /// </summary>
        public string ProcessPlatformVersion => Environment.Is64BitProcess ? "64bit" : "32bit";

        /// <summary>
        /// Gets the number of available cores.
        /// </summary>
        public string CpuCoresCount => Environment.ProcessorCount + " cores";
    }
}
