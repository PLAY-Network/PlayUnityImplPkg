using RGN.ImplDependencies.Core.Messaging;
using FirebaseMessageReceivedEventArgs = Firebase.Messaging.MessageReceivedEventArgs;

namespace RGN.Modules.Messaging.Runtime
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
