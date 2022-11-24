using RGN.Dependencies.Core;
using FirebaseFirestore = Firebase.Firestore.FirebaseFirestore;

namespace RGN.Impl.Firebase.Core
{
    public sealed class DocumentDB : IDocumentDB
    {
        private readonly FirebaseFirestore firebaseFirestore;

        internal DocumentDB(FirebaseFirestore firebaseFirestore)
        {
            this.firebaseFirestore = firebaseFirestore;
        }

        void IDocumentDB.UserEmulator(string hostAndPort, bool sslEnabled)
        {
            firebaseFirestore.Settings.Host = hostAndPort;
            firebaseFirestore.Settings.SslEnabled = sslEnabled;
        }
    }
}
