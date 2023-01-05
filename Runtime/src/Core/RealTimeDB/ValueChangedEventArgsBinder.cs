using RGN.Dependencies.Core.RealTimeDB;
using System;
using FirebaseQuery = Firebase.Database.Query;
using FirebaseValueChangedEventArgs = Firebase.Database.ValueChangedEventArgs;

namespace RGN.Impl.Firebase.Core.RealTimeDB
{
    internal sealed class ValueChangedEventArgsBinder : IDisposable
    {
        private readonly FirebaseQuery firebaseQuery;

        internal EventHandler<ValueChangedEventArgs> ToSubcribe { get; private set; }

        public ValueChangedEventArgsBinder(
            FirebaseQuery firebaseQuery,
            EventHandler<ValueChangedEventArgs> toSubcribe)
        {
            this.firebaseQuery = firebaseQuery;
            ToSubcribe = toSubcribe;
            firebaseQuery.ValueChanged += OnValueChanged;
        }
        public void Dispose()
        {
            ToSubcribe = null;
            firebaseQuery.ValueChanged -= OnValueChanged;
        }

        private void OnValueChanged(object sender, FirebaseValueChangedEventArgs e)
        {
            ValueChangedEventArgs eventArgs = null;
            if (e.DatabaseError != null)
            {
                eventArgs = new ValueChangedEventArgs(new DatabaseError(e.DatabaseError));
            }
            if (e.Snapshot != null)
            {
                eventArgs = new ValueChangedEventArgs(new DataSnapshot(e.Snapshot));
            }
            ToSubcribe.Invoke(sender, eventArgs);
        }
    }
}
