using System;
using RGN.ImplDependencies.Core.DynamicLinks;
using FirebaseDynamicLinks = Firebase.DynamicLinks.DynamicLinks;

namespace RGN.Modules.DynamicLinks.Runtime
{
    public class DynamicLinkReceivedEventArgsBinder : IDisposable
    {
        internal Action<object, IDynamicLinkReceivedEventArgs> ToSubscribe { get; private set; }

        public DynamicLinkReceivedEventArgsBinder(Action<object, IDynamicLinkReceivedEventArgs> toSubscribe)
        {
            ToSubscribe = toSubscribe;
            FirebaseDynamicLinks.DynamicLinkReceived += OnDynamicLinkReceived;
        }
        
        public void Dispose()
        {
            ToSubscribe = null;
            FirebaseDynamicLinks.DynamicLinkReceived -= OnDynamicLinkReceived;
        }

        private void OnDynamicLinkReceived(object sender, EventArgs args)
        {
            Firebase.DynamicLinks.ReceivedDynamicLinkEventArgs dynamicLinkEventArgs = args as Firebase.DynamicLinks.ReceivedDynamicLinkEventArgs;
            ToSubscribe.Invoke(sender, new DynamicLinkReceivedEventArgs(dynamicLinkEventArgs!.ReceivedDynamicLink));
        }
    }
}
