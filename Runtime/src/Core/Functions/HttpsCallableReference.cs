using RGN.Dependencies.Core.Functions;
using System;
using System.Threading.Tasks;
using FirebaseHttpsCallableReference = Firebase.Functions.HttpsCallableReference;

namespace RGN.Impl.Firebase.Core.Functions
{
    public sealed class HttpsCallableReference : IHttpsCallableReference
    {
        private readonly FirebaseHttpsCallableReference firebaseHttpsCallableReference;

        internal HttpsCallableReference(FirebaseHttpsCallableReference firebaseHttpsCallableReference)
        {
            this.firebaseHttpsCallableReference = firebaseHttpsCallableReference;
        }

        async Task<IHttpsCallableResult> IHttpsCallableReference.CallAsync()
        {
            var result = await firebaseHttpsCallableReference.CallAsync();
            return new HttpsCallableResult(result);
        }

        async Task<IHttpsCallableResult> IHttpsCallableReference.CallAsync(object data)
        {
            var result = await firebaseHttpsCallableReference.CallAsync(data);
            return new HttpsCallableResult(result);
        }
    }
}
