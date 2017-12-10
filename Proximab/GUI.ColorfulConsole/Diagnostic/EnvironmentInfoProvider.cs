using System;

namespace GUI.ColorfulConsole.Diagnostic
{
    /// <summary>
    /// Represents a set of methods for retrieving OS information.
    /// </summary>
    internal class EnvironmentInfoProvider
    {
        /// <summary>
        /// Gets the full operating system version.
        /// </summary>
        public string OSInfo
        {
             get { return Environment.OSVersion.VersionString; }
        }

        /// <summary>
        /// Gets the platform version (32/64 bits).
        /// </summary>
        public string CPUPlatformVersion
        {
             get { return Environment.Is64BitProcess ? "64bit" : "32bit"; }
        }

        /// <summary>
        /// Gets the process version (32/64 bits).
        /// </summary>
        public string ProcessPlatformVersion
        {
             get { return Environment.Is64BitProcess ? "64bit" : "32bit"; }
        }

        /// <summary>
        /// Gets the number of available cores.
        /// </summary>
        public string CPUCoresCount
        {
             get { return Environment.ProcessorCount + " cores"; }
        }
    }
}
