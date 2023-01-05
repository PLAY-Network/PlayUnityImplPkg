using RGN.Dependencies.Core.Auth;
using FirebaseOAuthProvider = Firebase.Auth.OAuthProvider;

namespace RGN.Impl.Firebase.Core.Auth
{
    public sealed class OAuthProvider : IOAuthProvider
    {
        ICredential IOAuthProvider.GetCredential(string providerId, string idToken, string accessToken)
        {
            var credential = FirebaseOAuthProvider.GetCredential(providerId, idToken, accessToken);
            return new CredentialWrapper(credential);
        }
        ICredential IOAuthProvider.GetCredential(string providerId, string idToken, string rawNonce, string accessToken)
        {
            var credential = FirebaseOAuthProvider.GetCredential(providerId, idToken, rawNonce, accessToken);
            return new CredentialWrapper(credential);
        }
    }
}
