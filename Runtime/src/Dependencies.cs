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
        public IApp ReadyMasterApp { get; }
        public IAnalytics Analytics { get; }
        public IAuth Auth { get; }
        public IAuth ReadyMasterAuth { get; }
        public IFunctions Fn { get; }
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
            ReadyMasterApp = new Core.App(readyMasterApp);

            Auth = new Core.Auth.Auth(FirebaseAuth.GetAuth(app));
            ReadyMasterAuth = new Core.Auth.Auth(FirebaseAuth.GetAuth(readyMasterApp));

            Json = new Serialization.Json();
            Fn = new Core.FunctionsHttpClient.Functions(Json, ReadyMasterAuth, ApplicationStore.GetRGNMasterProjectId);
            ReadyMasterFunction = new Core.FunctionsHttpClient.Functions(Json, ReadyMasterAuth, ApplicationStore.GetRGNMasterProjectId);

            DynamicLinks = new Core.DynamicLinks.DynamicLinks();
            Messaging = new Core.Messaging.Messaging();

            EngineApp = new Engine.EngineApp();
            Time = new Engine.Time();
            Logger = new Engine.Logger();
            Analytics = new Core.Analytics();
        }
    }
}
