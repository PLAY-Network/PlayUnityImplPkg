using RGN.Dependencies.Core.Storage;
using System;
using System.Threading;
using System.Threading.Tasks;
using FirebaseStorageReference = Firebase.Storage.StorageReference;
using FirebaseMetadataChange = Firebase.Storage.MetadataChange;
using FirebaseStorageMetadata = Firebase.Storage.StorageMetadata;

namespace RGN.Impl.Firebase.Core.Storage
{
    public sealed class StorageReference : IStorageReference
    {
        private readonly FirebaseStorageReference firebaseStorageReference;

        internal StorageReference(FirebaseStorageReference firebaseStorageReference)
        {
            this.firebaseStorageReference = firebaseStorageReference;
        }

        Task<byte[]> IStorageReference.GetBytesAsync(long maxDownloadSizeBytes)
        {
            return firebaseStorageReference.GetBytesAsync(maxDownloadSizeBytes);
        }

        Task<byte[]> IStorageReference.GetBytesAsync(
            long maxDownloadSizeBytes,
            IProgress<IDownloadState> progressHandler,
            CancellationToken cancelToken)
        {
            var firebaseProgressHandler = new DownloadProgressHandler(progressHandler, this);
            return firebaseStorageReference.GetBytesAsync(maxDownloadSizeBytes, firebaseProgressHandler, cancelToken);
        }

        async Task<IStorageMetadata> IStorageReference.PutBytesAsync(
            byte[] bytes,
            IMetadataChange customMetadata,
            IProgress<IUploadState> progressHandler,
            CancellationToken cancelToken,
            Uri previousSessionUri)
        {
            FirebaseMetadataChange firebaseMetadataChange = null;
            if (customMetadata != null)
            {
                firebaseMetadataChange = new FirebaseMetadataChange()
                {
                    ContentLanguage = customMetadata.ContentLanguage,
                    ContentEncoding = customMetadata.ContentEncoding,
                    ContentDisposition = customMetadata.ContentDisposition,
                    CacheControl = customMetadata.CacheControl,
                    CustomMetadata = customMetadata.CustomMetadata,
                    ContentType = customMetadata.ContentType,
                };
            }
            UploadProgressHandler firebaseProgressHandler = null;
            if (progressHandler != null)
            {
                firebaseProgressHandler = new UploadProgressHandler(progressHandler, this);
            }
            FirebaseStorageMetadata result = await firebaseStorageReference.PutBytesAsync(
                bytes,
                firebaseMetadataChange,
                firebaseProgressHandler,
                cancelToken,
                previousSessionUri);
            return new StorageMetadata(); // TODO: use the result
        }
    }
}
