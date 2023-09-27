using RGN.ImplDependencies.Core.Auth;
using System.Collections.Generic;
using System.Threading.Tasks;
using FirebaseCredential = Firebase.Auth.Credential;
using FirebaseUser = Firebase.Auth.FirebaseUser;

namespace RGN.Impl.Firebase.Core.Auth
{
    internal sealed class User : UserInfo, IUser
    {
        private readonly FirebaseUser mFirebaseUser;
        public bool IsAnonymous => mFirebaseUser != null ? mFirebaseUser.IsAnonymous : false;

        public IEnumerable<IUserInfo> ProviderData
        {
            get
            {
                List<IUserInfo> providers = new List<IUserInfo>();
                if (mAuth != null && mAuth.CurrentLoggedInUser.HasValue)
                {
                    providers.Add(new UserInfo(mAuth));
                }
                if (mFirebaseUser != null)
                {
                    foreach (var info in mFirebaseUser.ProviderData)
                    {
                        providers.Add(new UserInfo(info));
                    }
                }
                return providers;
            }
        }

        internal User(FirebaseUser firebaseUser)
            : base(firebaseUser)
        {
            Utility.ThrowIf.Argument.IsNull(firebaseUser, nameof(firebaseUser));
            mFirebaseUser = firebaseUser;
        }
        internal User(Auth auth)
            : base(auth)
        {
        }

        public async Task<ISignInResult> LinkAndRetrieveDataWithCredentialAsync(ICredential credential)
        {
            FirebaseCredential firebaseCredential = credential.Credential as FirebaseCredential;
            var result = await mFirebaseUser.LinkAndRetrieveDataWithCredentialAsync(firebaseCredential);
            return new SignInResult(result);
        }

        public Task<string> TokenAsync(bool forceRefresh)
        {
            if (mAuth != null)
            {
                return Task.FromResult(mAuth.CurrentLoggedInUser.Value.IdToken);
            }
            return mFirebaseUser.TokenAsync(forceRefresh);
        }
    }
}
