﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PackageVersion.cs" company="In The Hand Ltd">
//   Copyright (c) 2013-16 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace InTheHand.ApplicationModel
{
    /// <summary>
    /// Represents the package version info.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Android</term><description>Android 4.4 and later</description></item>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>macOS</term><description>OS X 10.7 and later</description></item>
    /// <item><term>tvOS</term><description>tvOS 9.0 and later</description></item>
    /// <item><term>watchOS</term><description>watchOS 2.0 and later</description></item>
    /// <item><term>Tizen</term><description>Tizen 3.0</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item>
    /// <item><term>Windows (Desktop Apps)</term><description>Windows 7 or later</description></item></list>
    /// </remarks>
    public partial struct PackageVersion
    {
        /// <summary>
        /// The major version number of the package.
        /// </summary>
        public ushort Major;
        /// <summary>
        /// The minor version number of the package.
        /// </summary>
        public ushort Minor;
        /// <summary>
        /// The build version number of the package.
        /// </summary>
        public ushort Build;
        /// <summary>
        /// The revision version number of the package.
        /// </summary>
        public ushort Revision;

        /// <summary>
        /// Returns a formatted string for the PackageVersion.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.ToVersion().ToString();
        }
    }
}