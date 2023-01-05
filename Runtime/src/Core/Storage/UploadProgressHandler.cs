using RGN.Dependencies.Core.Storage;
using System;
using FirebaseUploadStateState = Firebase.Storage.UploadState;

namespace RGN.Impl.Firebase.Core.Storage
{
    public sealed class UploadProgressHandler : IProgress<FirebaseUploadStateState>
    {
        private readonly IProgress<IUploadState> progress;
        private readonly UploadState downloadState;

        internal UploadProgressHandler(IProgress<IUploadState> progress, IStorageReference storageReference)
        {
            this.progress = progress;
        }

        public void Report(FirebaseUploadStateState value)
        {
            throw new NotImplementedException();
        }
    }
}
