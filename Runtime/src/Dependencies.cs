using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Firestore;
using Firebase.Functions;
using Firebase.Storage;
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
        public IApplicationStore ApplicationStore { get; }
        public IApp App { get; }
        public IApp ReadyMasterApp { get; }
        public IAnalytics Analytics { get; }
        public IAuth Auth { get; }
        public IAuth ReadyMasterAuth { get; }
        public IFunctions Fn { get; }
        public IFunctions ReadyMasterFunction { get; }
        public IStorage Storage { get; }
        public IStorage ReadyMasterStorage { get; }
        public IDocumentDB ReadyMasterFirestore { get; }
        public IRealTimeDB RealtimeDatabase { get; }
        public IDynamicLinks DynamicLinks { get; }
        public IMessaging Messaging { get; }
        public IJson Json { get; }
        public IEngineApp EngineApp { get; }
        public ITime Time { get; }
        public ILogger Logger { get; }

        public Dependencies()
            : this(RGN.ApplicationStore.LoadFromResources())
        {
        }
        public Dependencies(IApplicationStore applicationStore)
        {
            ApplicationStore = applicationStore;
            var app = FirebaseApp.DefaultInstance;
            App = new Core.App(app);
            AppOptions appOptions = new AppOptions()
            {
                DatabaseUrl = applicationStore.GetRGNMasterDatabaseUrl,
                AppId = applicationStore.GetRGNMasterAppID,
                ApiKey = applicationStore.GetRGNMasterApiKey,
                MessageSenderId = applicationStore.GetRGNMasterMessageSenderId,
                StorageBucket = applicationStore.GetRGNMasterStorageBucket,
                ProjectId = applicationStore.GetRGNMasterProjectId,
            };
            var readyMasterApp = FirebaseApp.Create(appOptions, RGNCore.READY_MASTER_APP_CONFIG_NAME);
            ReadyMasterApp = new Core.App(readyMasterApp);

            Auth = new Core.Auth.Auth(FirebaseAuth.GetAuth(app));
            ReadyMasterAuth = new Core.Auth.Auth(FirebaseAuth.GetAuth(readyMasterApp));

            Fn = new Core.Functions.Functions(FirebaseFunctions.GetInstance(app));
            ReadyMasterFunction = new Core.Functions.Functions(FirebaseFunctions.GetInstance(readyMasterApp));

            ReadyMasterFirestore = new Core.DocumentDB(FirebaseFirestore.GetInstance(readyMasterApp));
            RealtimeDatabase = new Core.RealTimeDB.RealTimeDB(FirebaseDatabase.GetInstance(app));

            Storage = new Core.Storage.Storage(FirebaseStorage.GetInstance(app));
            ReadyMasterStorage = new Core.Storage.Storage(FirebaseStorage.GetInstance(readyMasterApp, applicationStore.GetRGNStorageURL));

            DynamicLinks = new Core.DynamicLinks.DynamicLinks();
            Messaging = new Core.Messaging.Messaging();

            Json = new Serialization.Json();
            EngineApp = new Engine.EngineApp();
            Time = new Engine.Time();
            Logger = new Engine.Logger();
            Analytics = new Core.Analytics();
        }
    }
}
