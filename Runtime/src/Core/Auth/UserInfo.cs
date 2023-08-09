using RGN.Dependencies.Core.Auth;
using System;
using IFirebaseUserInfo = Firebase.Auth.IUserInfo;

namespace RGN.Impl.Firebase.Core.Auth
{
    internal class UserInfo : IUserInfo
    {
        protected readonly IFirebaseUserInfo mUserInfo;
        protected readonly Auth mAuth;

        public string DisplayName => mUserInfo != null ? mUserInfo.DisplayName : "unknown";
        public string Email => mUserInfo != null ? mUserInfo.Email : mAuth.CurrentLoggedInUser.Value.Email;
        public Uri PhotoUrl => mUserInfo?.PhotoUrl;
        public string ProviderId => mUserInfo != null ? mUserInfo.ProviderId : "password";
        public string UserId => mUserInfo != null ? mUserInfo.UserId : mAuth.CurrentLoggedInUser.Value.UserId;

        internal UserInfo(IFirebaseUserInfo userInfo)
        {
            mUserInfo = userInfo;
        }
        internal UserInfo(Auth auth)
        {
            mAuth = auth;
        }
    }
}
