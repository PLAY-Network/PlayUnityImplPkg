using RGN.Dependencies.Core.RealTimeDB;
using System;
using System.Collections.Generic;
using FirebaseQuery = Firebase.Database.Query;

namespace RGN.Impl.Firebase.Core.RealTimeDB
{
    public class Query : IQuery
    {
        private readonly FirebaseQuery firebaseQuery;
        private readonly List<ValueChangedEventArgsBinder> valueChangedListeners = new List<ValueChangedEventArgsBinder>();

        event EventHandler<ValueChangedEventArgs> IQuery.ValueChanged
        {
            add
            {
                EventHandler<ValueChangedEventArgs> toSubcribe = value;
                valueChangedListeners.Add(new ValueChangedEventArgsBinder(firebaseQuery, toSubcribe));
            }
            remove
            {
                EventHandler<ValueChangedEventArgs> toUnsubcribe = value;
                var listener = valueChangedListeners.Find((binder) => binder.ToSubcribe == toUnsubcribe);
                listener.Dispose();
                valueChangedListeners.Remove(listener);
            }
        }

        internal Query(FirebaseQuery firebaseQuery)
        {
            this.firebaseQuery = firebaseQuery;
        }
    }
}
