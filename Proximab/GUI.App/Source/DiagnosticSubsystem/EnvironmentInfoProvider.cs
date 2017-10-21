﻿using System;

namespace GUI.App.Source.DiagnosticSubsystem
{
    internal class EnvironmentInfoProvider
    {
        public string GetOSInfo()
        {
            return Environment.OSVersion.VersionString;
        }

        public string GetCPUPlatformVersion()
        {
            return Environment.Is64BitProcess ? "64bit" : "32bit";
        }

        public string GetProcessPlatformVersion()
        {
            return Environment.Is64BitProcess ? "64bit" : "32bit";
        }

        public string GetCPUCoresCount()
        {
            return Environment.ProcessorCount + " cores";
        }
    }
}