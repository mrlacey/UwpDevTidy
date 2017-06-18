// <copyright file="Options.cs" company="Matt Lacey Ltd.">
// Copyright (c) Matt Lacey Ltd. All Rights Reserved.
// </copyright>

using CommandLine;
using CommandLine.Text;

namespace UWPDevTidy
{
    public class Options
    {
        [Option('l', "list", DefaultValue = false, HelpText = "Show all apps")]
        public bool List { get; set; }

        [Option('u', "uninstall", DefaultValue = false, HelpText = "Uninstall all apps")]
        public bool Uninstall { get; set; }

        [Option('n', "nameStarts", HelpText = "Prefix of app name.")]
        public string NameStarting { get; set; }

        [Option('m', "maxCount", DefaultValue = int.MaxValue, HelpText = "The maximum number of apps to process.")]
        public int MaximumCount { get; set; }

        [Option('v', "verbose", DefaultValue = false, HelpText = "Show all details during execution.")]
        public bool Verbose { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, current => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
