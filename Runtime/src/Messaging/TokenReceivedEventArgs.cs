using RGN.ImplDependencies.Core.Messaging;
using FirebaseTokenReceivedEventArgs = Firebase.Messaging.TokenReceivedEventArgs;

namespace RGN.Modules.Messaging.Runtime
{
    public sealed class TokenReceivedEventArgs : ITokenReceivedEventArgs
    {
        public string Token { get; }

        internal TokenReceivedEventArgs(FirebaseTokenReceivedEventArgs firebaseTokenReceivedEventArgs)
        {
            Token = firebaseTokenReceivedEventArgs.Token;
        }
    }
}
