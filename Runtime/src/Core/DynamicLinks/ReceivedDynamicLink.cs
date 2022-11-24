using RGN.Dependencies.Core.DynamicLinks;
using System;
using FirebaseReceivedDynamicLink = Firebase.DynamicLinks.ReceivedDynamicLink;

namespace RGN.Impl.Firebase.Core.DynamicLinks
{
    public sealed class ReceivedDynamicLink : IReceivedDynamicLink
    {
        public Uri Url { get; }

        public LinkMatchStrength MatchStrength { get; }

        internal ReceivedDynamicLink(FirebaseReceivedDynamicLink firebaseReceivedDynamicLink)
        {
            Url = firebaseReceivedDynamicLink.Url;
            MatchStrength = (LinkMatchStrength)(int)firebaseReceivedDynamicLink.MatchStrength;
        }
    }
}
