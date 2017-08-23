﻿using System;
using Xamarin.Auth;

namespace Xamarin_GoogleAuth.Authentication
{
    public class GoogleAuthenticator
    {
        private const string AuthorizeUrl = "https://accounts.google.com/o/oauth2/v2/auth";
        private const string AccessTokenUrl = "https://www.googleapis.com/oauth2/v4/token";
        private const bool IsUsingNativeUI = true;

        private OAuth2Authenticator _auth;
        private IGoogleAuthenticationDelegate _authenticatorDelegate;

        public GoogleAuthenticator(string clientId, string scope, string redirectUrl, IGoogleAuthenticationDelegate authenticatorDelegate)
        {
            _authenticatorDelegate = authenticatorDelegate;

            _auth = new OAuth2Authenticator(clientId, string.Empty, scope,
                                            new Uri(AuthorizeUrl),
                                            new Uri(redirectUrl),
                                            new Uri(AccessTokenUrl),
                                            null, IsUsingNativeUI);

            _auth.Completed += OnAuthenticationCompleted;
            _auth.Error += OnAuthenticationFailed;
        }

        public OAuth2Authenticator GetAuthenticator()
        {
            return _auth;
        }

        public void OnPageLoading(Uri uri)
        {
            _auth.OnPageLoading(uri);
        }

        private void OnAuthenticationCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            var token = new GoogleOAuthToken
            {
                TokenType = e.Account.Properties["token_type"],
                AccessToken = e.Account.Properties["access_token"]
            };

            _authenticatorDelegate.OnAuthenticationCompleted(token);
        }

        private void OnAuthenticationFailed(object sender, AuthenticatorErrorEventArgs e)
        {
            _authenticatorDelegate.OnAuthenticationFailed(e.Message, e.Exception);
        }
    }
}
