using RGN.Dependencies.Core.Auth;
using FirebaseGoogleAuthProvider = Firebase.Auth.GoogleAuthProvider;

namespace RGN.Impl.Firebase.Core.Auth
{
    public sealed class GoogleAuthProvider : IGoogleAuthProvider
    {
        ICredential IGoogleAuthProvider.GetCredential(string idToken, string accessToken)
        {
            var credential = FirebaseGoogleAuthProvider.GetCredential(idToken, accessToken);
            return new CredentialWrapper(credential);
        }
    }
}
