using RGN.ImplDependencies.Core.Messaging;
using System;
using FirebaseMessaging = Firebase.Messaging.FirebaseMessaging;
using FirebaseTokenReceivedEventArgs = Firebase.Messaging.TokenReceivedEventArgs;

namespace RGN.Modules.Messaging.Runtime
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
