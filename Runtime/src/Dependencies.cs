using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Firestore;
using Firebase.Functions;
using Firebase.Storage;
using ReadyGamesNetwork.RGN.Dependencies.Engine;
using RGN.Dependencies;
using RGN.Dependencies.Core;
using RGN.Dependencies.Core.Auth;
using RGN.Dependencies.Core.DynamicLinks;
using RGN.Dependencies.Core.Functions;
using RGN.Dependencies.Core.Messaging;
using RGN.Dependencies.Core.RealTimeDB;
using RGN.Dependencies.Core.Storage;
using RGN.Dependencies.Engine;
using RGN.Dependencies.Serialization;

namespace RGN.Impl.Firebase
{
    public sealed class Dependencies : IDependencies
    {
        public IApp app { get; }
        public IApp readyMasterApp { get; }
        public IAuth auth { get; }
        public IAuth readyMasterAuth { get; }
        public IFunctions fn { get; }
        public IFunctions readyMasterFunction { get; }
        public IStorage storage { get; }
        public IStorage readyMasterStorage { get; }
        public IDocumentDB readyMasterFirestore { get; }
        public IRealTimeDB realtimeDatabase { get; }
        public IDynamicLinks dynamicLinks { get; }
        public IMessaging messaging { get; }
        public IJson Json { get; }
        public IEngineApp EngineApp { get; }
        public ITime Time { get; }
        public ILogger Logger { get; }

        public Dependencies(Core.AppOptions readyMasterAuthOptions, string readyMasterStorageUrl)
        {
            var app = FirebaseApp.DefaultInstance;
            this.app = new Core.App(app);
            AppOptions appOptions = new AppOptions()
            {
                DatabaseUrl = readyMasterAuthOptions.DatabaseUrl,
                AppId = readyMasterAuthOptions.AppId,
                ApiKey = readyMasterAuthOptions.ApiKey,
                MessageSenderId = readyMasterAuthOptions.MessageSenderId,
                StorageBucket = readyMasterAuthOptions.StorageBucket,
                ProjectId = readyMasterAuthOptions.ProjectId,
            };
            var readyMasterApp = FirebaseApp.Create(appOptions, "Secondary");
            this.readyMasterApp = new Core.App(readyMasterApp);

            auth = new Core.Auth.Auth(FirebaseAuth.GetAuth(app));
            readyMasterAuth = new Core.Auth.Auth(FirebaseAuth.GetAuth(readyMasterApp));

            fn = new Core.Functions.Functions(FirebaseFunctions.GetInstance(app));
            readyMasterFunction = new Core.Functions.Functions(FirebaseFunctions.GetInstance(readyMasterApp));

            readyMasterFirestore = new Core.DocumentDB(FirebaseFirestore.GetInstance(readyMasterApp));
            realtimeDatabase = new Core.RealTimeDB.RealTimeDB(FirebaseDatabase.GetInstance(app));

            storage = new Core.Storage.Storage(FirebaseStorage.GetInstance(app));
            readyMasterStorage = new Core.Storage.Storage(FirebaseStorage.GetInstance(readyMasterApp, readyMasterStorageUrl));

            dynamicLinks = new Core.DynamicLinks.DynamicLinks();
            messaging = new Core.Messaging.Messaging();

            Json = new Serialization.Json();
            EngineApp = new Engine.EngineApp();
            Time = new Engine.Time();
            Logger = new Engine.Logger();
        }
    }
}