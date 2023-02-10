using RGN.Dependencies.Core.Functions;

namespace RGN.Impl.Firebase.Core.FunctionsHttpClient
{
    public sealed class HttpsCallableResult : IHttpsCallableResult
    {
        public object Data { get; }

        internal HttpsCallableResult(object data)
        {
            Data = data;
        }
    }
}
