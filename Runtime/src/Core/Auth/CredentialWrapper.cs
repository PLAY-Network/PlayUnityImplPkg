using RGN.ImplDependencies.Core.Auth;
using FirebaseCredential = Firebase.Auth.Credential;

namespace RGN.Impl.Firebase.Core.Auth
{
    public sealed class CredentialWrapper : ICredential
    {
        public object Credential { get; }

        internal CredentialWrapper(FirebaseCredential firebaseCredential)
        {
            Credential = firebaseCredential;
        }
    }
}
