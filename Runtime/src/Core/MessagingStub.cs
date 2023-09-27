using RGN.ImplDependencies.Core.Messaging;
using System;

namespace RGN.Impl.Firebase.Core
{
    public sealed class MessagingStub : IMessaging
    {
        public event Action<object, ITokenReceivedEventArgs> TokenReceived;

        public event Action<object, IMessageReceivedEventArgs> MessageReceived;
    }
}
