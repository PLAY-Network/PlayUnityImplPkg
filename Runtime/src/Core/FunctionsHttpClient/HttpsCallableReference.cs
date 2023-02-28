using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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
        private readonly string mRngMasterProjectId;
        private readonly string mFunctionName;
        private readonly Uri mCallAddress;

        internal HttpsCallableReference(
            HttpClient httpClient,
            IJson json,
            IAuth readyMasterAuth,
            string rngMasterProjectId,
            string baseAddress,
            string functionName)
        {
            mHttpClient = httpClient;
            mJson = json;
            mReadyMasterAuth = readyMasterAuth;
            mRngMasterProjectId = rngMasterProjectId;
            mFunctionName = functionName;
            mCallAddress = new Uri(new Uri(baseAddress), functionName);
        }

        Task<IHttpsCallableResult> IHttpsCallableReference.CallAsync()
        {
            return CallInternalAsync(null);
        }
        Task<IHttpsCallableResult> IHttpsCallableReference.CallAsync(object data)
        {
            return CallInternalAsync(data);
        }
        Task<TResult> IHttpsCallableReference.CallAsync<TPayload, TResult>()
        {
            return CallInternalAsync<TPayload, TResult>(default);
        }
        Task<TResult> IHttpsCallableReference.CallAsync<TPayload, TResult>(TPayload payload)
        {
            return CallInternalAsync<TPayload, TResult>(payload);
        }

        private async Task<IHttpsCallableResult> CallInternalAsync(object data)
        {
            UnityEngine.Debug.Log(mCallAddress);
            var request = new HttpRequestMessage(
                    HttpMethod.Post,
                    mCallAddress /*"http://127.0.0.1:5001/readysandbox/us-central1/virtualItemsV2-getByAppId"*/);
            string jsonContent = data == null ? "{}" : mJson.ToJson(data);
            string body = $"{{\"data\": {jsonContent} }}";
            request.Content = new StringContent(
                body,
                Encoding.UTF8,
                "application/json");

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
                    string errorMessage = GetErrorMessage(message);
                    throw new HttpRequestException(errorMessage);
                }
                var strJson = await response.Content.ReadAsStringAsync();
                try
                {
                    var dict = mJson.FromJson<Dictionary<object, Dictionary<object, object>>>(strJson);
                    return new HttpsCallableResult(dict["result"]);
                }
                catch (Newtonsoft.Json.JsonSerializationException)
                {
                    var dict = mJson.FromJson<Dictionary<object, string>>(strJson);
                    return new HttpsCallableResult(dict["result"]);
                }
            }
        }

        private async Task<TResult> CallInternalAsync<TPayload, TResult>(TPayload payload)
        {
            var request = new HttpRequestMessage(
                    HttpMethod.Post,
                    mCallAddress /*"http://127.0.0.1:5001/readysandbox/us-central1/virtualItemsV2-getByAppId"*/);
            string jsonContent = payload == null ? "{}" : mJson.ToJson(payload);
            string body = $"{{\"data\": {jsonContent} }}";
            request.Content = new StringContent(
                body,
                Encoding.UTF8,
                "application/json");

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
                    string errorMessage = GetErrorMessage(message);
                    throw new HttpRequestException(errorMessage);
                }
                var stream = await response.Content.ReadAsStreamAsync();
                var dict = mJson.FromJson<Dictionary<object, TResult>>(stream);
                var result = dict["result"];
                return result;
            }
        }

        private string GetErrorMessage(string message)
        {
#if READY_DEVELOPMENT
            string urlToFunctionLog =
                        @$"https://console.cloud.google.com/logs/query;query=resource.type%3D%22
cloud_function%22%20resource.labels.function_name%3D%22{mFunctionName}%22?project={mRngMasterProjectId}&authuser=0&hl=en";
            string errorMessage = mFunctionName + ": " + message + ", url: " + urlToFunctionLog;
            return errorMessage;
#else
            return mFunctionName + ": " + message;
#endif
        }
    }
}
