using RGN.Dependencies.Core.Auth;
using System;
using IFirebaseUserInfo = Firebase.Auth.IUserInfo;

namespace RGN.Impl.Firebase.Core.Auth
{
    public class UserInfo : IUserInfo
    {
        protected readonly IFirebaseUserInfo userInfo;

        public string DisplayName => userInfo.DisplayName;

        public string Email => userInfo.Email;

        public Uri PhotoUrl => userInfo.PhotoUrl;

        public string ProviderId => userInfo.ProviderId;

        public string UserId => userInfo.UserId;

        internal UserInfo(IFirebaseUserInfo userInfo)
        {
            this.userInfo = userInfo;
        }
    }
}
