using RGN.Dependencies.Core.Storage;
using System.Collections.Generic;
using FirebaseMetadataChange = Firebase.Storage.MetadataChange;

namespace RGN.Impl.Firebase.Core.Storage
{
    public sealed class MetadataChange : IMetadataChange
    {
        private readonly FirebaseMetadataChange firebaseMetadataChange;

        string IMetadataChange.ContentLanguage
        {
            get => firebaseMetadataChange.ContentLanguage;
            set => firebaseMetadataChange.ContentLanguage = value;
        }
        string IMetadataChange.ContentEncoding
        {
            get => firebaseMetadataChange.ContentEncoding;
            set => firebaseMetadataChange.ContentEncoding = value;
        }
        string IMetadataChange.ContentDisposition
        {
            get => firebaseMetadataChange.ContentDisposition;
            set => firebaseMetadataChange.ContentDisposition = value;
        }
        string IMetadataChange.CacheControl
        {
            get => firebaseMetadataChange.CacheControl;
            set => firebaseMetadataChange.CacheControl = value;
        }
        IDictionary<string, string> IMetadataChange.CustomMetadata
        {
            get => firebaseMetadataChange.CustomMetadata;
            set => firebaseMetadataChange.CustomMetadata = value;
        }
        string IMetadataChange.ContentType
        {
            get => firebaseMetadataChange.ContentType;
            set => firebaseMetadataChange.ContentType = value;
        }

        internal MetadataChange(FirebaseMetadataChange firebaseMetadataChange)
        {
            this.firebaseMetadataChange = firebaseMetadataChange;
        }
    }
}
