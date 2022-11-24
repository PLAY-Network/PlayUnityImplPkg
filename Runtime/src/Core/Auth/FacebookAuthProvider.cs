using RGN.Dependencies.Core.Auth;
using FirebaseFacebookAuthProvider = Firebase.Auth.FacebookAuthProvider;

namespace RGN.Impl.Firebase.Core.Auth
{
    public sealed class FaceBookAuthProvider : IFaceBookAuthProvider
    {
        ICredential IFaceBookAuthProvider.GetCredential(string accessToken)
        {
            var credential = FirebaseFacebookAuthProvider.GetCredential(accessToken);
            return new CredentialWrapper(credential);
        }
    }
}
