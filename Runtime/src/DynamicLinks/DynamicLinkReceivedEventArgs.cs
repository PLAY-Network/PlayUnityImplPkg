using RGN.ImplDependencies.Core.DynamicLinks;
using FirebaseDynamicLink = Firebase.DynamicLinks.ReceivedDynamicLink;

namespace RGN.Modules.DynamicLinks.Runtime
{
    public class DynamicLinkReceivedEventArgs : IDynamicLinkReceivedEventArgs
    {
        public IDynamicLink ReceivedDynamicLink { get; }
        
        internal DynamicLinkReceivedEventArgs(FirebaseDynamicLink receivedFirebaseDynamicLink)
        {
            ReceivedDynamicLink = new DynamicLink(receivedFirebaseDynamicLink);
        }
    }
}
