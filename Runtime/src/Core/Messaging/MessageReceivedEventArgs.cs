using RGN.Dependencies.Core.Messaging;
using FirebaseMessageReceivedEventArgs = Firebase.Messaging.MessageReceivedEventArgs;

namespace RGN.Impl.Firebase.Core.Messaging
{
    public sealed class MessageReceivedEventArgs : IMessageReceivedEventArgs
    {
        public IMessage Message { get; }

        internal MessageReceivedEventArgs(FirebaseMessageReceivedEventArgs firebaseMessageReceivedEventArgs)
        {
            Message = new Message(firebaseMessageReceivedEventArgs.Message);
        }
    }
}
