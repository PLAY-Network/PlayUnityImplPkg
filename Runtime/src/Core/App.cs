using Firebase;
using RGN.Dependencies.Core;
using System.Threading.Tasks;
using DependencyStatus = RGN.Dependencies.Core.DependencyStatus;

namespace RGN.Impl.Firebase.Core
{
    public sealed class App : IApp
    {
        private readonly FirebaseApp firebaseApp;

        public App(FirebaseApp firebaseApp)
        {
            this.firebaseApp = firebaseApp;
        }

        async Task<DependencyStatus> IApp.CheckAndFixDependenciesAsync()
        {
            var status = await FirebaseApp.CheckAndFixDependenciesAsync();
            return (DependencyStatus)((int)status);
        }
    }
}
