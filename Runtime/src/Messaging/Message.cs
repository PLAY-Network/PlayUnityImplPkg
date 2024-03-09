using RGN.ImplDependencies.Core.Messaging;
using System.Collections.Generic;
using FirebaseMessage = Firebase.Messaging.FirebaseMessage;

namespace RGN.Modules.Messaging.Runtime
{
    public sealed class Message : IMessage
    {
        private readonly FirebaseMessage firebaseMessage;

        string IMessage.MessageId => firebaseMessage.MessageId;

        string IMessage.MessageType => firebaseMessage.MessageType;

        string IMessage.Priority => firebaseMessage.Priority;

        string IMessage.Error => firebaseMessage.Error;

        string IMessage.ErrorDescription => firebaseMessage.ErrorDescription;

        bool IMessage.NotificationOpened => firebaseMessage.NotificationOpened;

        byte[] IMessage.RawData => firebaseMessage.RawData;

        string IMessage.From => firebaseMessage.From;

        string IMessage.To => firebaseMessage.To;

        string IMessage.CollapseKey => firebaseMessage.CollapseKey;

        IDictionary<string, string> IMessage.Data => firebaseMessage.Data;

        internal Message(FirebaseMessage firebaseMessage)
        {
            this.firebaseMessage = firebaseMessage;
        }
    }
}
