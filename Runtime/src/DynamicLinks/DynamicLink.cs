using System;
using RGN.ImplDependencies.Core.DynamicLinks;
using FirebaseDynamicLink = Firebase.DynamicLinks.ReceivedDynamicLink;
using FirebaseLinkMatchStrength = Firebase.DynamicLinks.LinkMatchStrength;

namespace RGN.Modules.DynamicLinks.Runtime
{
    public class DynamicLink : IDynamicLink
    {
        private readonly FirebaseDynamicLink firebaseDynamicLink;

        public LinkMatchStrength MatchStrength => FirebaseLinkMatchStrengthConvertToRgn(firebaseDynamicLink.MatchStrength);
        public Uri Url => firebaseDynamicLink.Url;

        public DynamicLink(FirebaseDynamicLink firebaseDynamicLink)
        {
            this.firebaseDynamicLink = firebaseDynamicLink;
        }

        private LinkMatchStrength FirebaseLinkMatchStrengthConvertToRgn(
            FirebaseLinkMatchStrength firebaseLinkMatchStrength)
        {
            switch (firebaseLinkMatchStrength)
            {
                case FirebaseLinkMatchStrength.NoMatch: return LinkMatchStrength.NoMatch;
                case FirebaseLinkMatchStrength.PerfectMatch: return LinkMatchStrength.PerfectMatch;
                case FirebaseLinkMatchStrength.StrongMatch: return LinkMatchStrength.StrongMatch;
                case FirebaseLinkMatchStrength.WeakMatch: return LinkMatchStrength.WeakMatch;
                default: throw new NotImplementedException(nameof(firebaseLinkMatchStrength));
            }
        }
    }
}
