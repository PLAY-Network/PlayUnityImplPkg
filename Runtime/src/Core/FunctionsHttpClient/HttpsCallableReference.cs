using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Firebase.Auth;
using RGN.Dependencies.Core.Auth;
using RGN.Dependencies.Core.Functions;
using RGN.Dependencies.Serialization;

namespace RGN.Impl.Firebase.Core.FunctionsHttpClient
{
    public sealed class HttpsCallableReference : IHttpsCallableReference
    {
        private readonly HttpClient mHttpClient;
        private readonly IJson mJson;
        private readonly IAuth mReadyMasterAuth;
        private readonly Uri mCallAddress;

        internal HttpsCallableReference(HttpClient httpClient, IJson json, IAuth readyMasterAuth, Uri callAddress)
        {
            mHttpClient = httpClient;
            mJson = json;
            mReadyMasterAuth = readyMasterAuth;
            mCallAddress = callAddress;
        }

        Task<IHttpsCallableResult> IHttpsCallableReference.CallAsync()
        {
            return CallInternalAsync(null);
        }

        Task<IHttpsCallableResult> IHttpsCallableReference.CallAsync(object data)
        {
            return CallInternalAsync(data);
        }
        private async Task<IHttpsCallableResult> CallInternalAsync(object data)
        {
            var request = new HttpRequestMessage(
                    HttpMethod.Post,
                    mCallAddress /*"http://127.0.0.1:5001/readysandbox/us-central1/virtualItemsV2-getByAppId"*/);
            string jsonContent = data == null ? "" : mJson.ToJson(data);
            string body = $"{{\"data\": {jsonContent} }}";
            request.Content = new StringContent(
                body,
                Encoding.UTF8,
                "application/json");

            Stopwatch sw = Stopwatch.StartNew();
            if (mReadyMasterAuth.CurrentUser != null)
            {
                string token = await mReadyMasterAuth.CurrentUser.TokenAsync(false);
                request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + token);
            }
            using (var response = await mHttpClient.SendAsync(
                request,
                HttpCompletionOption.ResponseHeadersRead))
            {
                if (!response.IsSuccessStatusCode)
                {
                    string message = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException(message);
                }
                var stream = await response.Content.ReadAsStreamAsync();
                var dict = mJson.FromJson<Dictionary<object, Dictionary<object, object>>>(stream);
                return new HttpsCallableResult(dict["result"]);
            }
        }
    }
}
