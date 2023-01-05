using RGN.Dependencies.Core.RealTimeDB;
using System.Threading.Tasks;
using FirebaseDatabaseReference = Firebase.Database.DatabaseReference;

namespace RGN.Impl.Firebase.Core.RealTimeDB
{
    public sealed class DatabaseReference : Query, IDatabaseReference
    {
        private readonly FirebaseDatabaseReference firebaseDatabaseReference;

        internal DatabaseReference(FirebaseDatabaseReference firebaseDatabaseReference)
            : base(firebaseDatabaseReference)
        {
            this.firebaseDatabaseReference = firebaseDatabaseReference;
        }

        IDatabaseReference IDatabaseReference.Child(string pathString)
        {
            return new DatabaseReference(firebaseDatabaseReference.Child(pathString));
        }
        IDatabaseReference IDatabaseReference.Push()
        {
            return new DatabaseReference(firebaseDatabaseReference.Push());
        }
        Task IDatabaseReference.SetRawJsonValueAsync(string jsonValue)
        {
            return firebaseDatabaseReference.SetRawJsonValueAsync(jsonValue);
        }
    }
}
