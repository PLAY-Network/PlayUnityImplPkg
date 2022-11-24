using RGN.Dependencies.Core.Auth;
using FirebaseSignInResult = Firebase.Auth.SignInResult;

namespace RGN.Impl.Firebase.Core.Auth
{
    public sealed class SignInResult : ISignInResult
    {
        private readonly FirebaseSignInResult firebaseSignInResult;

        internal SignInResult(FirebaseSignInResult firebaseSignInResult)
        {
            this.firebaseSignInResult = firebaseSignInResult;
        }
    }
}
