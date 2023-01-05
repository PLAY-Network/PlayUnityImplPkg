using RGN.Dependencies.Core.Auth;
using System.Collections.Generic;
using System.Threading.Tasks;
using FirebaseCredential = Firebase.Auth.Credential;
using FirebaseUser = Firebase.Auth.FirebaseUser;

namespace RGN.Impl.Firebase.Core.Auth
{
    public sealed class User : UserInfo, IUser
    {
        FirebaseUser firebaseUser;
        public bool IsAnonymous => firebaseUser.IsAnonymous;

        public IEnumerable<IUserInfo> ProviderData
        {
            get
            {
                List<IUserInfo> providers = new List<IUserInfo>();
                foreach (var info in firebaseUser.ProviderData)
                {
                    providers.Add(new UserInfo(info));
                }
                return providers;
            }
        }

        internal User(FirebaseUser firebaseUser)
            : base(firebaseUser)
        {
            Utility.ThrowIf.Argument.IsNull(firebaseUser, nameof(firebaseUser));
            this.firebaseUser = firebaseUser;
        }

        public async Task<ISignInResult> LinkAndRetrieveDataWithCredentialAsync(ICredential credential)
        {
            FirebaseCredential firebaseCredential = credential.Credential as FirebaseCredential;
            var result = await firebaseUser.LinkAndRetrieveDataWithCredentialAsync(firebaseCredential);
            return new SignInResult(result);
        }

        public Task<string> TokenAsync(bool forceRefresh)
        {
            return firebaseUser.TokenAsync(forceRefresh);
        }
    }
}
