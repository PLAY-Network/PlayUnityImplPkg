using RGN.Dependencies.Core.Storage;
using FirebaseMetadataChange = Firebase.Storage.MetadataChange;
using FirebaseStorage = Firebase.Storage.FirebaseStorage;

namespace RGN.Impl.Firebase.Core.Storage
{
    public sealed class Storage : IStorage
    {
        private readonly FirebaseStorage firebaseStorage;

        internal Storage(FirebaseStorage firebaseStorage)
        {
            this.firebaseStorage = firebaseStorage;
        }

        IStorageReference IStorage.GetReference(string location)
        {
            var reference = firebaseStorage.GetReference(location);
            return new StorageReference(reference);
        }

        IStorageReference IStorage.GetReferenceFromUrl(string fullUrl)
        {
            var reference = firebaseStorage.GetReferenceFromUrl(fullUrl);
            return new StorageReference(reference);
        }

        IMetadataChange IStorage.NewMetadataChange()
        {
            var metadataChange = new FirebaseMetadataChange();
            return new MetadataChange(metadataChange);
        }
    }
}
