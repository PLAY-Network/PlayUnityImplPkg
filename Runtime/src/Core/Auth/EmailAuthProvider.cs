using RGN.Dependencies.Core.Auth;
using FirebaseEmailAuthProvider = Firebase.Auth.EmailAuthProvider;

namespace RGN.Impl.Firebase.Core.Auth
{
    public sealed class EmailAuthProvider : IEmailAuthProvider
    {
        ICredential IEmailAuthProvider.GetCredential(string email, string password)
        {
            var credential = FirebaseEmailAuthProvider.GetCredential(email, password);
            return new CredentialWrapper(credential);
        }
    }
}
