﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CurrentApp.cs" company="In The Hand Ltd">
//   Copyright (c) 2014-16 In The Hand Ltd, All rights reserved.
// </copyright>
// <summary>
//   Launch Store UI for the current app.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using System.Reflection;
using Windows.ApplicationModel;
#if WINDOWS_APP || WINDOWS_UWP || WINDOWS_PHONE_APP
using Windows.System;
#endif

namespace InTheHand.ApplicationModel.Store
{
    /// <summary>
    /// Defines methods and properties you can use to get license and listing info about the current app and perform in-app purchases.
    /// </summary>
    public static class CurrentApp
    {
#if WINDOWS_PHONE_APP
        private static bool _on10;

        static CurrentApp()
        {
            _on10 = typeof(Windows.ApplicationModel.Store.CurrentApp).GetRuntimeMethod("GetAppPurchaseCampaignIdAsync", new Type[0]) != null;
        }
#endif

        /// <summary>
        /// Gets the GUID generated by the Windows Store when your app has been certified for listing in the Windows Store.
        /// </summary>
        public static Guid AppId
        {
            get
            {
#if WINDOWS_APP || WINDOWS_UWP || WINDOWS_PHONE_APP
                return Windows.ApplicationModel.Store.CurrentApp.AppId;
//#elif WINDOWS_PHONE_APP
//                return new Guid(InTheHand.ApplicationModel.Package.Current.Id.ProductId);
#elif WINDOWS_PHONE
                // for Silverlight the FullName is a guid
                return new Guid(WMAppManifest.Current.ProductID);
#else
                return Guid.Empty;
#endif
            }
        }

        /// <summary>
        /// Creates the async operation that enables the user to view the app details.
        /// </summary>
        /// <returns></returns>
        public static Task<bool> RequestDetailsAsync()
        {
#if WINDOWS_APP || WINDOWS_UWP
            LauncherOptions options = new LauncherOptions();
            options.DesiredRemainingView = Windows.UI.ViewManagement.ViewSizePreference.UseMinimum;
            return Launcher.LaunchUriAsync(new Uri("ms-windows-store:PDP?PFN=" + Windows.ApplicationModel.Package.Current.Id.FamilyName), options).AsTask<bool>();
#elif WINDOWS_PHONE_APP
            if (_on10)
            {
                return Launcher.LaunchUriAsync(new Uri("ms-windows-store://pdp/?PhoneAppId=" + Windows.ApplicationModel.Package.Current.Id.ProductId)).AsTask<bool>();
            }
            else
            {
                return Launcher.LaunchUriAsync(new Uri("ms-windows-store:navigate?appid=" + AppId)).AsTask<bool>();
            }
#elif WINDOWS_PHONE
            return Windows.System.Launcher.LaunchUriAsync(new Uri("zune:navigate?appid=" + AppId)).AsTask<bool>();
#else
            return Task.Run<bool>(() => { return false; });
#endif
        }

        /// <summary>
        /// Creates the async operation that enables the user to review the current app.
        /// </summary>
        /// <returns></returns>
        public static Task<bool> RequestReviewAsync()
        {
#if WINDOWS_APP || WINDOWS_UWP
            LauncherOptions options = new LauncherOptions();
            options.DesiredRemainingView = Windows.UI.ViewManagement.ViewSizePreference.UseMinimum;
            return Launcher.LaunchUriAsync(new Uri("ms-windows-store:REVIEW?PFN=" + Windows.ApplicationModel.Package.Current.Id.FamilyName), options).AsTask<bool>();
#elif WINDOWS_PHONE_APP
            if (_on10)
            {
                return Launcher.LaunchUriAsync(new Uri("ms-windows-store://reviewapp/?AppId=" + Windows.ApplicationModel.Package.Current.Id.ProductId)).AsTask<bool>();
            }
            else
            {
                return Launcher.LaunchUriAsync(new Uri("ms-windows-store:reviewapp?appid=" + AppId)).AsTask<bool>();
            }
#elif WINDOWS_PHONE
            return Windows.System.Launcher.LaunchUriAsync(new Uri("zune:reviewapp?appid=app" + AppId)).AsTask<bool>();
#else
            return Task.Run<bool>(() => { return false; });
#endif
        }
    }
}
