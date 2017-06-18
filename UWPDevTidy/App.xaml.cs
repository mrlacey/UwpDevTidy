// <copyright file="App.xaml.cs" company="Matt Lacey Ltd.">
// Copyright (c) Matt Lacey Ltd. All Rights Reserved.
// </copyright>

using System;
using System.Linq;
using System.Windows;

namespace UWPDevTidy
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (e.Args.Any())
            {
                var options = new Options();

                if (CommandLine.Parser.Default.ParseArguments(e.Args, options))
                {
                    if (!NativeMethods.AttachConsole(-1))
                    {
                        // allocate a new console
                        NativeMethods.AllocConsole();
                    }

                    var defaultColor = Console.ForegroundColor;

                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine(options.List);
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(options.MaximumCount);
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine(options.NameStarting);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(options.Uninstall);
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(options.Verbose);

                    Console.ForegroundColor = defaultColor;

                    AppTidier.Tidy(options);
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
