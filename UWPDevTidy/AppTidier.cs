// <copyright file="AppTidier.cs" company="Matt Lacey Ltd.">
// Copyright (c) Matt Lacey Ltd. All Rights Reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Management.Automation;
using System.Xml;

namespace UWPDevTidy
{
    public static class AppTidier
    {
        public static void Tidy(Options options)
        {
            throw new NotImplementedException();
        }

        public static IEnumerable<string> GetAllAppNames()
        {
            var result = new List<string>();

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
                        //// outputItem.BaseOBject

                        if ((bool)outputItem.Properties["IsDevelopmentMode"].Value)
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
                                        Console.WriteLine(e);
                                    }

                                    break;
                                }
                            }

                            if (installLocation != null && Directory.Exists(installLocation))
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

                                    result.Add($"{displayName} ({officialName})");
                                }
                            }
                        }
                    }
                }
            }

            return result;
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
