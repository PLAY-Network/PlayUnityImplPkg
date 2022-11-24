using RGN.Dependencies.Core.Storage;
using System;
using FirebaseDownloadState = Firebase.Storage.DownloadState;

namespace RGN.Impl.Firebase.Core.Storage
{
    public sealed class DownloadProgressHandler : IProgress<FirebaseDownloadState>
    {
        private readonly IProgress<IDownloadState> progress;
        private readonly DownloadState downloadState;

        internal DownloadProgressHandler(IProgress<IDownloadState> progress, IStorageReference storageReference)
        {
            this.progress = progress;
            downloadState = new DownloadState(storageReference);
        }

        public void Report(FirebaseDownloadState value)
        {
            downloadState.TotalByteCount = value.TotalByteCount;
            downloadState.BytesTransferred = value.BytesTransferred;
            progress.Report(downloadState);
        }
    }
}
