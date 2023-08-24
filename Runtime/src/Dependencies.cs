using Firebase;
using Firebase.Auth;
using RGN.ImplDependencies.Core;
using RGN.ImplDependencies.Core.Auth;
using RGN.ImplDependencies.Core.Functions;
using RGN.ImplDependencies.Core.Messaging;
using RGN.ImplDependencies.Engine;
using RGN.ImplDependencies.Serialization;
using RGN.ModuleDependencies;

namespace RGN.Impl.Firebase
{
    public sealed class Dependencies : IDependencies
    {
        public IRGNAnalytics RGNAnalytics { get; }
        public IApplicationStore ApplicationStore { get; }
        public IApp App { get; }
        public IAnalytics Analytics { get; }
        public IAuth ReadyMasterAuth { get; }
        public IFunctions ReadyMasterFunction { get; }
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


            var readyMasterAuth = new Core.Auth.Auth(FirebaseAuth.GetAuth(readyMasterApp));
            ReadyMasterAuth = readyMasterAuth;
            Json = new Serialization.Json();
            ReadyMasterFunction = new Core.FunctionsHttpClient.Functions(Json, ReadyMasterAuth, ApplicationStore.GetRGNMasterProjectId);
            readyMasterAuth.SetFunctions(ReadyMasterFunction);

            Messaging = new Core.Messaging.Messaging();

            EngineApp = new Engine.EngineApp();
            Time = new Engine.Time();
            Logger = new Engine.Logger();
            Analytics = new Core.Analytics();
        }
    }
}
