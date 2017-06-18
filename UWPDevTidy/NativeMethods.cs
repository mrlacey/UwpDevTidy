// <copyright file="NativeMethods.cs" company="Matt Lacey Ltd.">
// Copyright (c) Matt Lacey Ltd. All Rights Reserved.
// </copyright>

using System.Runtime.InteropServices;

namespace UWPDevTidy
{
    public static class NativeMethods
    {
        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        public static extern bool AttachConsole(int pid);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int FreeConsole();
    }
}
