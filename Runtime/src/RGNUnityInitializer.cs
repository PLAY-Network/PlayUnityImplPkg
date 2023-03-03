using System.Threading.Tasks;
using RGN.Modules.SignIn;
using UnityEngine;

namespace RGN.Impl.Firebase
{
    public class RGNUnityInitializer : MonoBehaviour
    {
        [SerializeField] private bool _autoGuestLogin = true;
        [SerializeField] private bool _dontDestroyOnLoadNewScene = true;

        private bool _initialized = false;
        private async void Awake()
        {
            if (_dontDestroyOnLoadNewScene)
            {
                DontDestroyOnLoad(gameObject);
            }
            await InitializeAsync();
        }
        private void OnDestroy()
        {
            Dispose(true);
        }

        protected virtual async Task InitializeAsync()
        {
            if (_initialized)
            {
                return;
            }

#if UNITY_STANDALONE_WIN
            EmailSignInModule.InitializeWindowsDeepLink();
#endif

            RGNCoreBuilder.CreateInstance(new Dependencies());
            RGNCore.I.AuthenticationChanged += OnAuthenticationChanged;
            await RGNCoreBuilder.BuildAsync();
            _initialized = true;
        }
        protected virtual void Dispose(bool disposing)
        {
            RGNCoreBuilder.Dispose();
        }

        private void OnAuthenticationChanged(EnumLoginState enumLoginState, EnumLoginError error)
        {
            if (_autoGuestLogin && enumLoginState == EnumLoginState.NotLoggedIn)
            {
                Debug.Log("Automatically logging in as a guest");
                GuestSignInModule.I.TryToSignIn();
            }
        }
    }
}
