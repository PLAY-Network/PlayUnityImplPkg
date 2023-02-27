using System.Threading.Tasks;
using RGN.Impl.Firebase;
using UnityEngine;
using UnityEngine.UI;

namespace RGN.Samples
{
    public sealed class SettingsScreen : IUIScreen
    {
        [SerializeField] private Button _openSignInScreenButton;

        public override Task InitAsync(IRGNFrame rgnFrame)
        {
            base.InitAsync(rgnFrame);
            _openSignInScreenButton.onClick.AddListener(OnOpenSignInScreenButtonClick);
            return Task.CompletedTask;
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _openSignInScreenButton.onClick.RemoveListener(OnOpenSignInScreenButtonClick);
        }

        private void OnOpenSignInScreenButtonClick()
        {
            _rgnFrame.OpenScreen<SignInUpExample>();
        }
    }
}
