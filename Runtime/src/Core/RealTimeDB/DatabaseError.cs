using RGN.Dependencies.Core.RealTimeDB;
using FirebaseDatabaseError = Firebase.Database.DatabaseError;

namespace RGN.Impl.Firebase.Core.RealTimeDB
{
    public sealed class DatabaseError : IDatabaseError
    {
        private readonly FirebaseDatabaseError firebaseDatabaseError;

        int IDatabaseError.Code => firebaseDatabaseError.Code;
        string IDatabaseError.Message => firebaseDatabaseError.Message;
        string IDatabaseError.Details => firebaseDatabaseError.Details;

        internal DatabaseError(FirebaseDatabaseError firebaseDatabaseError)
        {
            this.firebaseDatabaseError = firebaseDatabaseError;
        }
    }
}
