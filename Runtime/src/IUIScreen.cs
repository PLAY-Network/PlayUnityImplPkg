using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace RGN.Impl.Firebase
{
    public abstract class IUIScreen : MonoBehaviour, System.IDisposable
    {
        [SerializeField] private Button _backButton;

        public RectTransform RectTransform { get; private set; }

        protected IRGNFrame _rgnFrame;

        public virtual Task InitAsync(IRGNFrame rgnFrame)
        {
            RectTransform = GetComponent<RectTransform>();
            _rgnFrame = rgnFrame;
            if (_backButton != null)
            {
                _backButton.gameObject.SetActive(false);
                _backButton.onClick.AddListener(OnBackButtonClick);
            }
            return Task.CompletedTask;
        }
        protected virtual void Dispose(bool disposing)
        {
            if (_backButton != null)
            {
                _backButton.onClick.RemoveListener(OnBackButtonClick);
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
        }

        internal void SetVisible(bool visible, bool showBackButton)
        {
            if (_backButton != null)
            {
                _backButton.gameObject.SetActive(showBackButton);
            }
            gameObject.SetActive(visible);
        }
        protected void OnBackButtonClick()
        {
            _rgnFrame.CloseTopScreen();
        }
    }
}
