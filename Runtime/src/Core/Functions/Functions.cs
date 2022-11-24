using RGN.Dependencies.Core.Functions;
using FirebaseFunctions = Firebase.Functions.FirebaseFunctions;

namespace RGN.Impl.Firebase.Core.Functions
{
    public sealed class Functions : IFunctions
    {
        private readonly FirebaseFunctions firebaseFunctions;

        internal Functions(FirebaseFunctions firebaseFunctions)
        {
            this.firebaseFunctions = firebaseFunctions;
        }

        IHttpsCallableReference IFunctions.GetHttpsCallable(string name)
        {
            var callable = firebaseFunctions.GetHttpsCallable(name);
            return new HttpsCallableReference(callable);
        }
        void IFunctions.UseFunctionsEmulator(string hostAndPort)
        {
            firebaseFunctions.UseFunctionsEmulator(hostAndPort);
        }
    }
}
