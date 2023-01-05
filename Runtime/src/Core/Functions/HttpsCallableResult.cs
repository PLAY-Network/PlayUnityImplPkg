using RGN.Dependencies.Core.Functions;
using FirebaseHttpsCallableResult = Firebase.Functions.HttpsCallableResult;

namespace RGN.Impl.Firebase.Core.Functions
{
    public sealed class HttpsCallableResult : IHttpsCallableResult
    {
        private readonly FirebaseHttpsCallableResult firebaseHttpsCallableResult;

        internal HttpsCallableResult(FirebaseHttpsCallableResult firebaseHttpsCallableResult)
        {
            this.firebaseHttpsCallableResult = firebaseHttpsCallableResult;
        }

        object IHttpsCallableResult.Data => firebaseHttpsCallableResult.Data;
    }
}
