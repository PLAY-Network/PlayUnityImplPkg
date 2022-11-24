using RGN.Dependencies.Core.Messaging;
using System;
using System.Collections.Generic;

namespace RGN.Impl.Firebase.Core.Messaging
{
    public sealed class Messaging : IMessaging
    {
        private readonly List<TokenReceivedEventArgsBinder> tokenListeners = new List<TokenReceivedEventArgsBinder>();
        private readonly List<MessageReceivedEventArgsBinder> messageListeners = new List<MessageReceivedEventArgsBinder>();

        event Action<object, ITokenReceivedEventArgs> IMessaging.TokenReceived
        {
            add
            {
                Action<object, ITokenReceivedEventArgs> toSubscribe = value;
                tokenListeners.Add(new TokenReceivedEventArgsBinder(toSubscribe));
            }
            remove
            {
                Action<object, ITokenReceivedEventArgs> toUnsubcribe = value;
                var listener = tokenListeners.Find((binder) => binder.ToSubcribe == toUnsubcribe);
                listener.Dispose();
                tokenListeners.Remove(listener);
            }
        }

        event Action<object, IMessageReceivedEventArgs> IMessaging.MessageReceived
        {
            add
            {
                Action<object, IMessageReceivedEventArgs> toSubscribe = value;
                messageListeners.Add(new MessageReceivedEventArgsBinder(toSubscribe));
            }
            remove
            {
                Action<object, IMessageReceivedEventArgs> toUnsubcribe = value;
                var listener = messageListeners.Find((binder) => binder.ToSubcribe == toUnsubcribe);
                listener.Dispose();
                messageListeners.Remove(listener);
            }
        }
    }
}
