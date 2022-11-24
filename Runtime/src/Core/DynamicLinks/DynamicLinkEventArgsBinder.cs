using RGN.Dependencies.Core.DynamicLinks;
using System;
using FirebaseDynamicLinks = Firebase.DynamicLinks.DynamicLinks;
using FirebaseReceivedDynamicLinkEventArgs = Firebase.DynamicLinks.ReceivedDynamicLinkEventArgs;

namespace RGN.Impl.Firebase.Core.DynamicLinks
{
    internal sealed class DynamicLinkEventArgsBinder : IDisposable
    {
        internal Action<object, IReceivedDynamicLinkEventArgs> ToSubcribe { get; private set; }

        public DynamicLinkEventArgsBinder(Action<object, IReceivedDynamicLinkEventArgs> toSubcribe)
        {
            ToSubcribe = toSubcribe;
            FirebaseDynamicLinks.DynamicLinkReceived += OnDynamicLinkReceived;
        }
        public void Dispose()
        {
            ToSubcribe = null;
            FirebaseDynamicLinks.DynamicLinkReceived -= OnDynamicLinkReceived;
        }

        private void OnDynamicLinkReceived(
            object sender,
            FirebaseReceivedDynamicLinkEventArgs eventArgs)
        {
            ToSubcribe.Invoke(sender, new ReceivedDynamicLinkEventArgs(eventArgs));
        }
    }
}
