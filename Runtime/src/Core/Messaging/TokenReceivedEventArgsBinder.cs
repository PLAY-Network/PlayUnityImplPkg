using RGN.Dependencies.Core.Messaging;
using System;
using FirebaseMessaging = Firebase.Messaging.FirebaseMessaging;
using FirebaseTokenReceivedEventArgs = Firebase.Messaging.TokenReceivedEventArgs;

namespace RGN.Impl.Firebase.Core.Messaging
{
    internal sealed class TokenReceivedEventArgsBinder : IDisposable
    {
        internal Action<object, ITokenReceivedEventArgs> ToSubcribe { get; private set; }

        public TokenReceivedEventArgsBinder(Action<object, ITokenReceivedEventArgs> toSubcribe)
        {
            ToSubcribe = toSubcribe;
            FirebaseMessaging.TokenReceived += OnTokenReceived;
        }
        public void Dispose()
        {
            ToSubcribe = null;
            FirebaseMessaging.TokenReceived -= OnTokenReceived;
        }

        private void OnTokenReceived(
            object sender,
            FirebaseTokenReceivedEventArgs eventArgs)
        {
            ToSubcribe.Invoke(sender, new TokenReceivedEventArgs(eventArgs));
        }
    }
}
