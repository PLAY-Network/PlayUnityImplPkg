using RGN.Dependencies.Core;
using System;

namespace RGN.Impl.Firebase.Core
{
    public sealed class AppOptions : IAppOptions
    {
        public Uri DatabaseUrl { get; set; }
        public string AppId { get; set; }
        public string ApiKey { get; set; }
        public string MessageSenderId { get; set; }
        public string StorageBucket { get; set; }
        public string ProjectId { get; set; }
    }
}
