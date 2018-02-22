// <copyright file="AppTidier.cs" company="Matt Lacey Ltd.">
// Copyright (c) Matt Lacey Ltd. All Rights Reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Xml;

namespace UWPDevTidy
{
    public static class AppTidier
    {
        public static void Tidy(Options options)
        {
            options.VerboseLog("Retrieving apps that can be uninstalledd");

            var allApps = GetAllAppNames();

            List<AppDetail> appsOfInterest = allApps;

            options.VerboseLog($"Found {appsOfInterest.Count} entries.");

            if (!string.IsNullOrWhiteSpace(options.NameStarting))
            {
                options.VerboseLog($"Filtering to apps starting '{options.NameStarting}'.");

                appsOfInterest = allApps.Where(a => a.DisplayName.StartsWith(options.NameStarting, StringComparison.CurrentCultureIgnoreCase)).ToList();

                options.VerboseLog($"App list now contains '{appsOfInterest.Count}' entries.");
            }

            if (options.List)
            {
                options.VerboseLog($"Listing {Math.Min(options.MaximumCount, appsOfInterest.Count)} apps.");

                foreach (var app in appsOfInterest.Take(options.MaximumCount))
                {
                    Console.ForegroundColor = App.DefaultColor;
                    Console.WriteLine(app.DisplayName);
                    Console.WriteLine(app.ProductFamilyName);
                    Console.WriteLine(app.InstallPath);
                    Console.WriteLine();
                }
            }

            if (options.Uninstall)
            {
                options.VerboseLog($"About to uninstall {Math.Min(options.MaximumCount, appsOfInterest.Count())} apps.");

                foreach (var app in appsOfInterest.Take(options.MaximumCount))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Uninstalling: {app.DisplayName} ({app.ProductFamilyName})");
                    RemoveApp(app.ProductFamilyName);
                    Console.WriteLine($"{app.DisplayName} uninstalled.");
                }
            }
        }

        public static List<AppDetail> GetAllAppNames()
        {
            var result = new List<AppDetail>();

            using (var ps = PowerShell.Create())
            {
                ps.AddScript("Get-AppxPackage");

                Collection<PSObject> psOutput = ps.Invoke();

                if (ps.Streams.Error.Count > 0)
                {
                    // error records were written to the error stream.
                    // do something with the items found.
                }

                // loop through each output object item
                foreach (PSObject outputItem in psOutput)
                {
                    // if null object was dumped to the pipeline during the script then a null
                    // object may be present here. check for null to prevent potential NRE.
                    if (outputItem != null)
                    {
                        //// TODO: do something with the output item
                        //// outputItem.BaseObject

                        if (outputItem.Properties.Any(p => p.Name == "IsDevelopmentMode")
                         && (bool)outputItem.Properties["IsDevelopmentMode"].Value)
                        {
                            string installLocation = null;

                            foreach (var outputItemMember in outputItem.Members)
                            {
                                if (outputItemMember.Name == "InstallLocation")
                                {
                                    try
                                    {
                                        installLocation = outputItemMember.Value.ToString();
                                        Debug.WriteLine(installLocation);
                                    }
                                    catch (Exception e)
                                    {
                                         Debug.WriteLine(e);
                                    }

                                    break;
                                }
                            }

                            if (installLocation != null && !installLocation.Contains("ShadowCache") && Directory.Exists(installLocation))
                            {
                                var manifestPath = Path.Combine(installLocation, "AppxManifest.xml");

                                if (File.Exists(manifestPath))
                                {
                                    var manifest = File.ReadAllText(manifestPath);

                                    var xml = new XmlDocument();
                                    xml.LoadXml(manifest);

                                    var displayName = xml.DocumentElement?.GetElementsByTagName("DisplayName")[0].InnerText ?? "*Unknown*";
                                    var officialName = outputItem.Properties["Name"].Value.ToString();

                                    Debug.WriteLine(displayName);

                                    result.Add(new AppDetail(displayName, officialName, installLocation));
                                }
                            }
                        }
                    }
                }
            }

            return result.OrderBy(a => a.DisplayName).ToList();
        }

        public static void RemoveApp(string appName)
        {
            using (var ps = PowerShell.Create())
            {
                ps.AddScript($"Get-AppxPackage {appName} | Remove-AppxPackage");

                Collection<PSObject> psOutput = ps.Invoke();

                if (ps.Streams.Error.Count > 0)
                {
                    // error records were written to the error stream.
                    // do something with the items found.
                }

                // loop through each output object item
                foreach (PSObject outputItem in psOutput)
                {
                    // if null object was dumped to the pipeline during the script then a null
                    // object may be present here. check for null to prevent potential NRE.
                    if (outputItem != null)
                    {
                        // TODO: do something with the output item
                        // outputItem.BaseOBject
                    }
                }
            }
        }
    }
}
