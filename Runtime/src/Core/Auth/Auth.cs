using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Auth;
using RGN.Dependencies.Core.Auth;
using RGN.Dependencies.Core.Functions;
using UnityEngine;

namespace RGN.Impl.Firebase.Core.Auth
{
    internal struct UserTokenInfo
    {
        public string Email { get; }
        public string UserId { get; }
        public string IdToken { get; }
        public string RefreshToken { get; }

        public UserTokenInfo(
            string email,
            string userId,
            string idToken,
            string refreshToken)
        {
            Email = email;
            UserId = userId;
            IdToken = idToken;
            RefreshToken = refreshToken;
        }
    }

    internal sealed class Auth : IAuth
    {
        private readonly FirebaseAuth mFirebaseAuth;
        private IFunctions _functions;

        internal UserTokenInfo? CurrentLoggedInUser { get; private set; }
        private readonly Dictionary<string, UserTokenInfo> mUserTokensCache =
            new Dictionary<string, UserTokenInfo>();
        private readonly HashSet<EventHandler> mListeners = new HashSet<EventHandler>();

        IUser IAuth.CurrentUser
        {
            get
            {
                if (CurrentLoggedInUser.HasValue)
                {
                    return new User(this);
                }
                return mFirebaseAuth.CurrentUser == null ? null : new User(mFirebaseAuth.CurrentUser); //TODO: cache it
            }
        }

        event EventHandler IAuth.StateChanged
        {
            add
            {
                mFirebaseAuth.StateChanged += value;
                mListeners.Add(value);
            }
            remove
            {
                mFirebaseAuth.StateChanged -= value;
                mListeners.Remove(value);
            }
        }

        internal Auth(FirebaseAuth firebaseAuth)
        {
            mFirebaseAuth = firebaseAuth;
        }
        internal void SetFunctions(IFunctions functions)
        {
            _functions = functions;
        }

        Task IAuth.SendPasswordResetEmailAsync(string email)
        {
            return mFirebaseAuth.SendPasswordResetEmailAsync(email);
        }

        async Task<IUser> IAuth.SignInWithCustomTokenAsync(string token)
        {
            var user = await mFirebaseAuth.SignInWithCustomTokenAsync(token);
            return new User(user);
        }

        async Task<IUser> IAuth.SignInWithEmailAndPasswordAsync(string email, string password)
        {
            if (!mUserTokensCache.TryGetValue(email, out UserTokenInfo userInfo))
            {
                userInfo = await SignInWithEmailAndPasswordAsync(email, password);
                mUserTokensCache.Add(email, userInfo);
            }
            CurrentLoggedInUser = userInfo;
            foreach (var listener in mListeners)
            {
                listener.Invoke(this, null);
            }
            return new User(this);
        }

        private async Task<UserTokenInfo> SignInWithEmailAndPasswordAsync(string email, string password)
        {
            var functionAuthenticate = _functions.GetHttpsCallable("user-signInWithEmailPassword");
            var response = await functionAuthenticate.CallAsync<Dictionary<string, object>, Dictionary<string, object>>(
                new Dictionary<string, object>() {
                    { "email", email },
                    { "password", password },
                    { "returnSecureToken", true }
                });
            string userId = (string)response["userId"];
            string idToken = (string)response["idToken"];
            string refreshToken = (string)response["refreshToken"];
            return new UserTokenInfo(email, userId, idToken, refreshToken);
        }

        void IAuth.SignOut()
        {
            if (CurrentLoggedInUser != null)
            {
                CurrentLoggedInUser = null;
                foreach (var listener in mListeners)
                {
                    listener.Invoke(this, null);
                }
                return;
            }
            mFirebaseAuth.SignOut();
        }
    }
}
