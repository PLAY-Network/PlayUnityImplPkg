using System.Threading.Tasks;
using RGN.Impl.Firebase;
using UnityEngine;
using UnityEngine.UI;

namespace RGN.Samples
{
    public class UIRoot : IUIScreen, IUserProfileClient
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Button _loginButton;
        [SerializeField] private Button _exploreUserProfileButton;
        [SerializeField] private Button _exploreVirtualItemsButton;
        [SerializeField] private Button _settingsButton;

        public override void PreInit(IRGNFrame rgnFrame)
        {
            base.PreInit(rgnFrame);
            _loginButton.onClick.AddListener(OnLoginButtonClick);
            _exploreUserProfileButton.onClick.AddListener(OnExploreUserProfileButtonClick);
            _exploreVirtualItemsButton.onClick.AddListener(OnExploreVirtualItemsButtonClick);
            _settingsButton.onClick.AddListener(OnSettingsButtonClick);
            _canvasGroup.interactable = false;
            SetUserLoggedIn(false);
            RGNCore.I.AuthenticationChanged += OnAuthenticationChanged;
        }
        public override Task InitAsync()
        {
            base.InitAsync();
            _rgnFrame.GetScreen<UserProfileExample>().SetUserProfileClient(this);
            return Task.CompletedTask;
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _loginButton.onClick.RemoveListener(OnLoginButtonClick);
            _exploreUserProfileButton.onClick.RemoveListener(OnExploreUserProfileButtonClick);
            _exploreVirtualItemsButton.onClick.RemoveListener(OnExploreVirtualItemsButtonClick);
            _settingsButton.onClick.RemoveListener(OnSettingsButtonClick);
            RGNCore.I.AuthenticationChanged -= OnAuthenticationChanged;
        }

        private void OnAuthenticationChanged(EnumLoginState state, EnumLoginError error)
        {
            SetUserLoggedIn(state == EnumLoginState.Success &&
                RGNCore.I.AuthorizedProviders == EnumAuthProvider.Email);
            _canvasGroup.interactable = true;
        }
        private void OnLoginButtonClick()
        {
            _rgnFrame.OpenScreen<SignInUpExample>();
        }
        private void OnSettingsButtonClick()
        {
            _rgnFrame.OpenScreen<SettingsScreen>();
        }
        private void OnExploreUserProfileButtonClick()
        {
            _rgnFrame.OpenScreen<UserProfileExample>();
        }
        private void OnExploreVirtualItemsButtonClick()
        {
            _rgnFrame.OpenScreen<VirtualItemsExample>();
        }

        Task<string> IUserProfileClient.GetPrimaryWalletAddressAsync()
        {
            return _rgnFrame.GetScreen<WalletsExample>().GetPrimaryWalletAddressAsync();
        }
        Task IUserProfileClient.OpenWalletsScreenAsync()
        {
            _rgnFrame.OpenScreen<WalletsExample>();
            return Task.CompletedTask;
        }
        private void SetUserLoggedIn(bool loggedInWithEmail)
        {
            _loginButton.gameObject.SetActive(!loggedInWithEmail);
            _exploreUserProfileButton.gameObject.SetActive(loggedInWithEmail);
            _exploreVirtualItemsButton.gameObject.SetActive(loggedInWithEmail);
        }
    }
}
