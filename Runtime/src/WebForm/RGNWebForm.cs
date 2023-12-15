﻿using System;
using System.Collections.Specialized;
using RGN.DeepLink;
using RGN.ImplDependencies.WebForm;
using UnityEngine;
#if UNITY_IOS && !UNITY_EDITOR
using RGN.DeepLink.iOS;
#endif

namespace RGN.WebForm
{
    public sealed class RGNWebForm : IWebForm
    {
        private WebFormSignInRedirectDelegate _onWebFormSignInRedirect;
        private WebFormCreateWalletRedirectDelegate _onWebFormCreateWalletRedirect;
        
        public void SignIn(WebFormSignInRedirectDelegate redirectCallback, string idToken)
        {
            _onWebFormSignInRedirect = redirectCallback;
            string redirectUrl = RGNDeepLinkHttpUtility.GetDeepLinkRedirectScheme();
            string url = GetWebFormUrl(redirectUrl) +
                         "&returnSecureToken=false" +
                         "&idToken=" + idToken;
            OpenWebForm(url, redirectUrl);
        }

        public void CreateWallet(WebFormCreateWalletRedirectDelegate redirectCallback, string idToken)
        {
            _onWebFormCreateWalletRedirect = redirectCallback;
            string redirectUrl = RGNDeepLinkHttpUtility.GetDeepLinkRedirectScheme();
            string url = GetWebFormUrl(redirectUrl) +
                         "&returnSecureToken=false" +
                         "&idToken=" + idToken +
                         "&view=createwallet";
            OpenWebForm(url, redirectUrl);
        }

        private void OpenWebForm(string url, string redirectUrl)
        {
#if UNITY_EDITOR
            RGNCore.I.Dependencies.DeepLink.StartEmulatorWatcher(redirectUrl);
#endif
            RGNCore.I.Dependencies.DeepLink.OnDeepLinkEvent += OnDeepLink;
            
            ApplicationFocusWatcher appFocusWatcher = ApplicationFocusWatcher.Create(delay: 1f);
            appFocusWatcher.OnFocusChanged += OnAppFocusChanged;

#if UNITY_IOS && !UNITY_EDITOR
            WebViewPlugin.ChangeURLScheme(redirectUrl);
            WebViewPlugin.OpenURL(url);
#else
            Application.OpenURL(url);
#endif
        }

        private void OnAppFocusChanged(ApplicationFocusWatcher appFocusWatcher, bool hasFocus)
        {
            if (hasFocus)
            {
                appFocusWatcher.OnFocusChanged -= OnAppFocusChanged;
                appFocusWatcher.Destroy();

                _onWebFormSignInRedirect?.Invoke(true, "");
                _onWebFormCreateWalletRedirect?.Invoke(true);
            }
        }
        
        private void OnDeepLink(string url)
        {
            if (_onWebFormSignInRedirect != null)
            {
                string parameters = url.Split("?"[0])[1];
                NameValueCollection parsedParameters = RGNDeepLinkHttpUtility.ParseQueryString(parameters);
                string token = parsedParameters["token"];
                _onWebFormSignInRedirect.Invoke(false, token);
                _onWebFormSignInRedirect = null;
            }
            
            _onWebFormCreateWalletRedirect?.Invoke(false);
            _onWebFormCreateWalletRedirect = null;
        }

        private string GetWebFormUrl(string redirectUrl) =>
            GetBaseWebFormUrl() +
            redirectUrl +
            "&appId=" + RGNCore.I.AppIDForRequests +
            "&lang=" + Utility.LanguageUtility.GetISO631Dash1CodeFromSystemLanguage();

        private string GetBaseWebFormUrl()
        {
            ApplicationStore applicationStore = ApplicationStore.LoadFromResources();
            return applicationStore.GetRGNEnvironment switch {
                EnumRGNEnvironment.Development => applicationStore.GetRGNDevelopmentEmailSignInURL,
                EnumRGNEnvironment.Staging => applicationStore.GetRGNStagingEmailSignInURL,
                EnumRGNEnvironment.Production => applicationStore.GetRGNProductionEmailSignInURL,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}