using Firebase;
using Firebase.Auth;
using RGN.Dependencies;
using RGN.Dependencies.Core;
using RGN.Dependencies.Core.Auth;
using RGN.Dependencies.Core.DynamicLinks;
using RGN.Dependencies.Core.Functions;
using RGN.Dependencies.Core.Messaging;
using RGN.Dependencies.Engine;
using RGN.Dependencies.Serialization;

namespace RGN.Impl.Firebase
{
    public sealed class Dependencies : IDependencies
    {
        public IApplicationStore ApplicationStore { get; }
        public IApp App { get; }
        public IAnalytics Analytics { get; }
        public IAuth ReadyMasterAuth { get; }
        public IFunctions ReadyMasterFunction { get; }
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


            var readyMasterAuth = new Core.Auth.Auth(FirebaseAuth.GetAuth(readyMasterApp));
            ReadyMasterAuth = readyMasterAuth;
            Json = new Serialization.Json();
            ReadyMasterFunction = new Core.FunctionsHttpClient.Functions(Json, ReadyMasterAuth, ApplicationStore.GetRGNMasterProjectId);
            readyMasterAuth.SetFunctions(ReadyMasterFunction);

            DynamicLinks = new Core.DynamicLinks.DynamicLinks();
            Messaging = new Core.Messaging.Messaging();

            EngineApp = new Engine.EngineApp();
            Time = new Engine.Time();
            Logger = new Engine.Logger();
            Analytics = new Core.Analytics();
        }
    }
}
