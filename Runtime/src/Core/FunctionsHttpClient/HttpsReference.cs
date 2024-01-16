using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using RGN.ImplDependencies.Core.Auth;
using RGN.ImplDependencies.Core.Functions;
using RGN.ImplDependencies.Serialization;
using RGN.Network;

namespace RGN.Impl.Firebase.Core.FunctionsHttpClient
{
    public sealed class HttpsReference : IHttpsCallableReference
    {
        private const string EMPTY_JSON = "{}";
        private const int COLD_START_EMULATE_DELAY = 10000;
        
        private readonly IJson mJson;
        private readonly IAuth mReadyMasterAuth;
        private readonly string mRngMasterProjectId;
        private readonly string mApiKey;
        private readonly string mFunctionName;
        private readonly Uri mCallAddress;
        private readonly bool mActAsACallable;
        private readonly bool mComputeHmac;

        internal HttpsReference(
            IJson json,
            IAuth readyMasterAuth,
            string rngMasterProjectId,
            string apiKey,
            string baseAddress,
            string functionName,
            bool actAsACallable,
            bool computeHmac)
        {
            mJson = json;
            mReadyMasterAuth = readyMasterAuth;
            mRngMasterProjectId = rngMasterProjectId;
            mApiKey = apiKey;
            mFunctionName = functionName;
            mCallAddress = new Uri(new Uri(baseAddress), functionName);
            mActAsACallable = actAsACallable;
            mComputeHmac = computeHmac;
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
#if READY_DEVELOPMENT && EMULATE_COLDSTART
            var callSw = new Stopwatch();
            callSw.Start();
#endif
            UnityEngine.Debug.Log(mCallAddress);
            var request = new HttpRequestMessage(
                    HttpMethod.Post,
                    mCallAddress);
            string jsonContent = EMPTY_JSON;
            if (data != null)
            {
                if (data is string)
                {
                    jsonContent = data as string;
                }
                else
                {
                    jsonContent = mJson.ToJson(data);
                }
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
            if (mComputeHmac)
            {
                string hmac = ComputeHmac(mApiKey, content);
                request.Headers.TryAddWithoutValidation("hmac", hmac);
            }
            string appId = RGNCore.I.AppIDForRequests;
            if (!string.IsNullOrWhiteSpace(appId))
            {
                request.Headers.TryAddWithoutValidation("app-id", appId);
            }
            using HttpClient httpClient = HttpClientFactory.Get();
            using HttpResponseMessage response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            if (!response.IsSuccessStatusCode)
            {
                string message = await response.Content.ReadAsStringAsync();
                string errorMessage = GetErrorMessage(message);
                throw new HttpRequestException(errorMessage);
            }
            await response.Content.ReadAsStringAsync();
#if READY_DEVELOPMENT && EMULATE_COLDSTART
            callSw.Stop();
            int delayToReachColdStart = Math.Max(0, COLD_START_EMULATE_DELAY - (int)callSw.ElapsedMilliseconds);
            await Task.Delay(delayToReachColdStart);
#endif
        }

        private async Task<TResult> CallInternalAsync<TPayload, TResult>(TPayload payload)
        {
#if READY_DEVELOPMENT && EMULATE_COLDSTART
            var callSw = new Stopwatch();
            callSw.Start();
#endif
            UnityEngine.Debug.Log(mCallAddress);
            var request = new HttpRequestMessage(
                    HttpMethod.Post,
                    mCallAddress);
            string jsonContent = EMPTY_JSON;
            if (payload != null)
            {
                if (payload is string)
                {
                    jsonContent = payload as string;
                }
                else
                {
                    jsonContent = mJson.ToJson(payload);
                }
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
            if (mComputeHmac)
            {
                string hmac = ComputeHmac(mApiKey, content);
                request.Headers.TryAddWithoutValidation("hmac", hmac);
            }
            string appId = RGNCore.I.AppIDForRequests;
            if (!string.IsNullOrWhiteSpace(appId))
            {
                request.Headers.TryAddWithoutValidation("app-id", appId);
            }
            using HttpClient httpClient = HttpClientFactory.Get();
            using HttpResponseMessage response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
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
#if READY_DEVELOPMENT && EMULATE_COLDSTART
            callSw.Stop();
            int delayToReachColdStart = Math.Max(0, COLD_START_EMULATE_DELAY - (int)callSw.ElapsedMilliseconds);
            await Task.Delay(delayToReachColdStart);
#endif
            if (mActAsACallable)
            {
                var dict = mJson.FromJson<Dictionary<object, TResult>>(stream);
                var result = dict["result"];
                return result;
            }
            return mJson.FromJson<TResult>(stream);
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
        private string ComputeHmac(string secret, string message)
        {
            var key = Encoding.UTF8.GetBytes(secret);
            using (var hasher = new HMACSHA256(key))
            {
                var messageBytes = Encoding.UTF8.GetBytes(message);
                var hash = hasher.ComputeHash(messageBytes);
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }
    }
}
