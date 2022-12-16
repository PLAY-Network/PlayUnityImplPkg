using Firebase.Auth;
using RGN.Dependencies.Core.Auth;
using System;
using System.Threading.Tasks;
using FirebaseCredential = Firebase.Auth.Credential;

namespace RGN.Impl.Firebase.Core.Auth
{
    public sealed class Auth : IAuth
    {
        private readonly FirebaseAuth firebaseAuth;

        IUser IAuth.CurrentUser => firebaseAuth.CurrentUser == null ? null : new User(firebaseAuth.CurrentUser); //TODO: cache it

        public IFaceBookAuthProvider faceBookAuthProvider { get; set; }
        public IEmailAuthProvider emailAuthProvider { get; set; }
        public IGoogleAuthProvider googleAuthProvider { get; set; }
        public IOAuthProvider oAuthProvider { get; set; }

        event EventHandler IAuth.StateChanged
        {
            add
            {
                firebaseAuth.StateChanged += value;
            }
            remove
            {
                firebaseAuth.StateChanged -= value;
            }
        }

        internal Auth(FirebaseAuth firebaseAuth)
        {
            this.firebaseAuth = firebaseAuth;
            faceBookAuthProvider = new FaceBookAuthProvider();
            emailAuthProvider = new EmailAuthProvider();
            googleAuthProvider = new GoogleAuthProvider();
            oAuthProvider = new OAuthProvider();
        }

        Task IAuth.SendPasswordResetEmailAsync(string email)
        {
            return firebaseAuth.SendPasswordResetEmailAsync(email);
        }

        async Task<IUser> IAuth.SignInAnonymouslyAsync()
        {
            var user = await firebaseAuth.SignInAnonymouslyAsync();
            return new User(user);
        }

        async Task<IUser> IAuth.SignInWithCredentialAsync(ICredential credential)
        {
            FirebaseCredential firebaseCredential = credential.Credential as FirebaseCredential;
            var user = await firebaseAuth.SignInWithCredentialAsync(firebaseCredential);
            return new User(user);
        }

        async Task<IUser> IAuth.SignInWithCustomTokenAsync(string token)
        {
            var user = await firebaseAuth.SignInWithCustomTokenAsync(token);
            return new User(user);
        }

        async Task<IUser> IAuth.SignInWithEmailAndPasswordAsync(string email, string password)
        {
            var user = await firebaseAuth.SignInWithEmailAndPasswordAsync(email, password);
            return new User(user);
        }

        void IAuth.SignOut()
        {
            firebaseAuth.SignOut();
        }
    }
}
