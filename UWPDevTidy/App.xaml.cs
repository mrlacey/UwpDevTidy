// <copyright file="App.xaml.cs" company="Matt Lacey Ltd.">
// Copyright (c) Matt Lacey Ltd. All Rights Reserved.
// </copyright>

using System;
using System.Linq;
using System.Windows;
using CommandLine.Text;

namespace UWPDevTidy
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public static ConsoleColor DefaultColor { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (e.Args.Any())
            {
                if (!NativeMethods.AttachConsole(-1))
                {
                    // allocate a new console
                    NativeMethods.AllocConsole();
                }

                var options = new Options();

                if (CommandLine.Parser.Default.ParseArguments(e.Args, options))
                {
                    DefaultColor = Console.ForegroundColor;

                    var appTitle = new HelpText
                    {
                        Heading = HeadingInfo.Default,
                        Copyright = CopyrightInfo.Default,
                        AdditionalNewLineAfterOption = true,
                        AddDashesToOption = true
                    };

                    Console.WriteLine(appTitle);

                    AppTidier.Tidy(options);

                    Console.ForegroundColor = DefaultColor;
                }
                else
                {
                    Console.WriteLine(options.GetUsage());
                }

                NativeMethods.FreeConsole();
                Environment.Exit(0);
            }
            else
            {
                new MainWindow().ShowDialog();
            }

            this.Shutdown();
        }
    }
}
