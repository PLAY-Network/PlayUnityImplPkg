using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Firebase;
using RGN.ImplDependencies.Core;
using DependencyStatus = RGN.ImplDependencies.Core.DependencyStatus;

namespace RGN.Impl.Firebase
{
    public class App : IApp, IImpl<IApp>
    {
        private static string sCachedValue;

        public async Task<DependencyStatus> CheckAndFixDependenciesAsync(CancellationToken cancellationToken = default)
        {
            var status = await FirebaseApp.CheckAndFixDependenciesAsync();
            cancellationToken.ThrowIfCancellationRequested();
            return (DependencyStatus)((int)status);
        }

        public string GetFirebaseSdkVersion()
        {
            if (sCachedValue != null)
            {
                return sCachedValue;
            }

            Type versionInfoType = Type.GetType("Firebase.VersionInfo, Firebase.App");
            if (versionInfoType != null)
            {
                PropertyInfo sdkVersionProperty =
                    versionInfoType.GetProperty("SdkVersion", BindingFlags.NonPublic | BindingFlags.Static);
                if (sdkVersionProperty != null)
                {
                    sCachedValue = sdkVersionProperty.GetValue(null) as string;
                    return sCachedValue;
                }
            }
            return "unknown";
        }
    }
}
