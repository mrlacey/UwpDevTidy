// <copyright file="AppDetail.cs" company="Matt Lacey Ltd.">
// Copyright (c) Matt Lacey Ltd. All Rights Reserved.
// </copyright>

namespace UWPDevTidy
{
    public class AppDetail
    {
        public AppDetail()
        {
        }

        public AppDetail(string displayName, string officialName, string installLocation)
        {
            this.DisplayName = displayName;
            this.ProductFamilyName = officialName;
            this.InstallPath = installLocation;
        }

        public string DisplayName { get; set; }

        public string ProductFamilyName { get; set; }

        public string InstallPath { get; set; }
    }
}
