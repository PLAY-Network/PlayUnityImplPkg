using RGN.Modules.SignIn;
using UnityEngine;

namespace RGN.Impl.Firebase
{
    public class RGNUnityInitializer : MonoBehaviour
    {
        [SerializeField] private bool _autoGuestLogin = true;

        private bool _initialized = false;
        private async void Awake()
        {
            if (_initialized)
            {
                return;
            }
            RGNCoreBuilder.CreateInstance(new Dependencies());
            RGNCore.I.AuthenticationChanged += OnAuthenticationChanged;
            await RGNCoreBuilder.BuildAsync();
            _initialized = true;
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
