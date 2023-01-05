using RGN.Dependencies.Core.DynamicLinks;
using FirebaseReceivedDynamicLinkEventArgs = Firebase.DynamicLinks.ReceivedDynamicLinkEventArgs;

namespace RGN.Impl.Firebase.Core.DynamicLinks
{
    public sealed class ReceivedDynamicLinkEventArgs : IReceivedDynamicLinkEventArgs
    {
        public IReceivedDynamicLink ReceivedDynamicLink { get; }

        internal ReceivedDynamicLinkEventArgs(FirebaseReceivedDynamicLinkEventArgs firebaseReceivedDynamicLinkEventArgs)
        {
            ReceivedDynamicLink = new ReceivedDynamicLink(firebaseReceivedDynamicLinkEventArgs.ReceivedDynamicLink);
        }
    }
}
