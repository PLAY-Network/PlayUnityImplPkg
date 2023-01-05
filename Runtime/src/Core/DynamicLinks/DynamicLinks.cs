using RGN.Dependencies.Core.DynamicLinks;
using System;
using System.Collections.Generic;

namespace RGN.Impl.Firebase.Core.DynamicLinks
{
    public sealed class DynamicLinks : IDynamicLinks
    {
        private readonly List<DynamicLinkEventArgsBinder> listeners = new List<DynamicLinkEventArgsBinder>();

        public event Action<object, IReceivedDynamicLinkEventArgs> DynamicLinkReceived
        {
            add
            {
                Action<object, IReceivedDynamicLinkEventArgs> toSubcribe = value;
                listeners.Add(new DynamicLinkEventArgsBinder(toSubcribe));
            }
            remove
            {
                Action<object, IReceivedDynamicLinkEventArgs> toUnsubcribe = value;
                var listener = listeners.Find((binder) => binder.ToSubcribe == toUnsubcribe);
                listener.Dispose();
                listeners.Remove(listener);
            }
        }
    }
}
