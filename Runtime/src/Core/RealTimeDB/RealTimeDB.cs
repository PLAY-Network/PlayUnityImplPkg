using RGN.Dependencies.Core.RealTimeDB;
using FirebaseDatabase = Firebase.Database.FirebaseDatabase;

namespace RGN.Impl.Firebase.Core.RealTimeDB
{
    public sealed class RealTimeDB : IRealTimeDB
    {
        private readonly FirebaseDatabase firebaseDatabase;

        IDatabaseReference IRealTimeDB.RootReference => new DatabaseReference(firebaseDatabase.RootReference);

        internal RealTimeDB(FirebaseDatabase firebaseDatabase)
        {
            this.firebaseDatabase = firebaseDatabase;
        }
    }
}
