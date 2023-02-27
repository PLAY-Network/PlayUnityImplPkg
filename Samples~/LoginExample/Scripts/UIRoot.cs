using System.Threading.Tasks;
using RGN.Impl.Firebase;
using UnityEngine;
using UnityEngine.UI;

namespace RGN.Samples
{
    public class UIRoot : IUIScreen
    {
        [SerializeField] private Button _exploreButton;
        [SerializeField] private Button _settingsButton;

        public override Task InitAsync(IRGNFrame rgnFrame)
        {
            base.InitAsync(rgnFrame);
            _exploreButton.onClick.AddListener(OnExploreButtonClick);
            _settingsButton.onClick.AddListener(OnSettingsButtonClick);
            RGNCore.I.AuthenticationChanged += OnAuthenticationChanged;
            return Task.CompletedTask;
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _exploreButton.onClick.RemoveListener(OnExploreButtonClick);
            _settingsButton.onClick.RemoveListener(OnSettingsButtonClick);
            RGNCore.I.AuthenticationChanged -= OnAuthenticationChanged;
        }

        private void OnAuthenticationChanged(EnumLoginState state, EnumLoginError error)
        {
        }
        private void OnExploreButtonClick()
        {
            if (RGNCore.I.AuthorizedProviders == EnumAuthProvider.Email)
            {
                _rgnFrame.OpenScreen<WalletsExample>();
            }
            else
            {
                _rgnFrame.OpenScreen<SignInUpExample>();
            }
        }
        private void OnSettingsButtonClick()
        {
            _rgnFrame.OpenScreen<SettingsScreen>();
        }
    }
}
