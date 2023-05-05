using System.Net.Http;
using RGN.Dependencies.Core.Auth;
using RGN.Dependencies.Core.Functions;
using RGN.Dependencies.Serialization;
using RGN.Network;

namespace RGN.Impl.Firebase.Core.FunctionsHttpClient
{
    public sealed class Functions : IFunctions
    {
        private const string REGION = "us-central1";

        private readonly IJson mJson;
        private readonly IAuth mReadyMasterAuth;
        private readonly string mRngMasterProjectId;
        private string _baseCloudAddress;

        internal Functions(IJson json, IAuth readyMasterAuth, string rngMasterProjectId)
        {
            mJson = json;
            mReadyMasterAuth = readyMasterAuth;
            mRngMasterProjectId = rngMasterProjectId;
            _baseCloudAddress = $"https://{REGION}-{mRngMasterProjectId}.cloudfunctions.net/";
        }

        IHttpsCallableReference IFunctions.GetHttpsCallable(string name)
        {
            HttpClient newClient = HttpClientFactory.Get();
            return new HttpsReference(
                newClient,
                mJson,
                mReadyMasterAuth,
                mRngMasterProjectId,
                _baseCloudAddress,
                name,
                true);
        }
        IHttpsCallableReference IFunctions.GetHttpsRequest(string name)
        {
            HttpClient newClient = HttpClientFactory.Get();
            return new HttpsReference(
                newClient,
                mJson,
                mReadyMasterAuth,
                mRngMasterProjectId,
                _baseCloudAddress,
                name,
                false);
        }
        void IFunctions.UseFunctionsEmulator(string hostAndPort)
        {
            _baseCloudAddress = $"http://{hostAndPort}/{mRngMasterProjectId}/{REGION}/";
        }
    }
}
