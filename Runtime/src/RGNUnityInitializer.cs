using System.Collections;
using System.Threading.Tasks;
using RGN.Modules.SignIn;
using UnityEngine;

namespace RGN.Impl.Firebase
{
    public class RGNUnityInitializer : MonoSingleton<RGNUnityInitializer>
    {
        [SerializeField] private bool _autoGuestLogin = true;

        protected override async void OnAwakeInternal()
        {
            await InitializeAsync();
        }
        protected override void OnDestroyInternal()
        {
            Dispose(true);
        }

        protected virtual async Task InitializeAsync()
        {
            if (RGNCoreBuilder.Initialized)
            {
                return;
            }

#if UNITY_STANDALONE_WIN
            EmailSignInModule.InitializeWindowsDeepLink();
#endif

            RGNCoreBuilder.CreateInstance(new Dependencies());
            RGNCore.I.AuthenticationChanged += OnAuthenticationChanged;
            await RGNCoreBuilder.BuildAsync();
        }
        protected virtual void Dispose(bool disposing)
        {
            RGNCoreBuilder.Dispose();
        }

        private void OnAuthenticationChanged(AuthState authState)
        {
            if (_autoGuestLogin && authState.LoginState == EnumLoginState.NotLoggedIn)
            {
                StartCoroutine(CallTryToLoginAfterAFrame());
            }
        }
        private IEnumerator CallTryToLoginAfterAFrame()
        {
            yield return null;
            Debug.Log("Automatically logging in as a guest");
            GuestSignInModule.I.TryToSignInAsync();
        }
    }
}
