using RGN.Dependencies.Core.Storage;

namespace RGN.Impl.Firebase.Core.Storage
{
    public sealed class DownloadState : IDownloadState
    {
        public long BytesTransferred { get; internal set; }
        public long TotalByteCount { get; internal set; }
        public IStorageReference Reference { get; }

        internal DownloadState(IStorageReference storageReference)
        {
            Reference = storageReference;
        }
    }
}
