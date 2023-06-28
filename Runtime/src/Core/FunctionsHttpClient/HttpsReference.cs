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
    public sealed class HttpsReference : IHttpsCallableReference
    {
        private const string EMPTY_JSON = "{}";

        private readonly HttpClient mHttpClient;
        private readonly IJson mJson;
        private readonly IAuth mReadyMasterAuth;
        private readonly string mRngMasterProjectId;
        private readonly string mFunctionName;
        private readonly Uri mCallAddress;
        private readonly bool mActAsACallable;

        internal HttpsReference(
            HttpClient httpClient,
            IJson json,
            IAuth readyMasterAuth,
            string rngMasterProjectId,
            string baseAddress,
            string functionName,
            bool actAsACallable)
        {
            mHttpClient = httpClient;
            mJson = json;
            mReadyMasterAuth = readyMasterAuth;
            mRngMasterProjectId = rngMasterProjectId;
            mFunctionName = functionName;
            mCallAddress = new Uri(new Uri(baseAddress), functionName);
            mActAsACallable = actAsACallable;
        }

        Task IHttpsCallableReference.CallAsync()
        {
            return CallInternalAsync(null);
        }
        Task IHttpsCallableReference.CallAsync(object data)
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

        private async Task CallInternalAsync(object data)
        {
            UnityEngine.Debug.Log(mCallAddress);
            var request = new HttpRequestMessage(
                    HttpMethod.Post,
                    mCallAddress);
            string jsonContent = EMPTY_JSON;
            if (data != null)
            {
                jsonContent = mJson.ToJson(data);
            }
            string content = jsonContent;
            if (mActAsACallable)
            {
                content = $"{{\"data\": {jsonContent} }}";
            }
            request.Content = new StringContent(
                content,
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
                await response.Content.ReadAsStringAsync();
            }
        }

        private async Task<TResult> CallInternalAsync<TPayload, TResult>(TPayload payload)
        {
            var request = new HttpRequestMessage(
                    HttpMethod.Post,
                    mCallAddress);
            string jsonContent = EMPTY_JSON;
            if (payload != null)
            {
                jsonContent = mJson.ToJson(payload);
            }
            string content = jsonContent;
            if (mActAsACallable)
            {
                content = $"{{\"data\": {jsonContent} }}";
            }
            request.Content = new StringContent(
                content,
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
                if (typeof(TResult) == typeof(string))
                {
                    string result = await response.Content.ReadAsStringAsync();
                    return (TResult)(object)result;
                }
                var stream = await response.Content.ReadAsStreamAsync();
                if (mActAsACallable)
                {
                    var dict = mJson.FromJson<Dictionary<object, TResult>>(stream);
                    var result = dict["result"];
                    return result;
                }
                return mJson.FromJson<TResult>(stream);
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
