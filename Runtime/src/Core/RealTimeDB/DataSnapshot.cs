using RGN.Dependencies.Core.RealTimeDB;
using FirebaseDataSnapshot = Firebase.Database.DataSnapshot;

namespace RGN.Impl.Firebase.Core.RealTimeDB
{
    public sealed class DataSnapshot : IDataSnapshot
    {
        private readonly FirebaseDataSnapshot firebaseDataSnapshot;

        object IDataSnapshot.Value => firebaseDataSnapshot.Value;
        bool IDataSnapshot.Exists => firebaseDataSnapshot.Exists;

        internal DataSnapshot(FirebaseDataSnapshot firebaseDataSnapshot)
        {
            this.firebaseDataSnapshot = firebaseDataSnapshot;
        }
    }
}
