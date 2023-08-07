using Firebase.Auth;
using RGN.Dependencies.Core.Auth;
using RGN.Dependencies.Core.Functions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RGN.Impl.Firebase.Core.Auth
{
    public sealed class Auth : IAuth
    {
        private readonly FirebaseAuth mFirebaseAuth;
        private IFunctions _functions;

        IUser IAuth.CurrentUser => mFirebaseAuth.CurrentUser == null ? null : new User(mFirebaseAuth.CurrentUser); //TODO: cache it

        event EventHandler IAuth.StateChanged
        {
            add
            {
                mFirebaseAuth.StateChanged += value;
            }
            remove
            {
                mFirebaseAuth.StateChanged -= value;
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
            string customToken = await SignInWithEmailAndPasswordAsync(email, password);
            var user = await mFirebaseAuth.SignInWithCustomTokenAsync(customToken);
            return new User(user);
        }

        private async Task<string> SignInWithEmailAndPasswordAsync(string email, string password)
        {
            var functionAuthenticate = _functions.GetHttpsCallable("user-signInWithEmailPassword");
            var response = await functionAuthenticate.CallAsync<Dictionary<string, string>, Dictionary<string, object>>(
                new Dictionary<string, string>() {
                    { "email", email },
                    { "password", password }
                });
            string customToken = (string)response["customToken"];
            return customToken;
        }

        void IAuth.SignOut()
        {
            mFirebaseAuth.SignOut();
        }
    }
}
