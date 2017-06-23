// <copyright file="OptionsExtensions.cs" company="Matt Lacey Ltd.">
// Copyright (c) Matt Lacey Ltd. All Rights Reserved.
// </copyright>

using System;

namespace UWPDevTidy
{
    public static class OptionsExtensions
    {
        public static void VerboseLog(this Options options, string message)
        {
            if (options.Verbose)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine(message);
            }
        }
    }
}
