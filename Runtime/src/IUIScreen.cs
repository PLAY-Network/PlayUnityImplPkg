using System.Threading.Tasks;
using UnityEngine;

namespace RGN.Impl.Firebase
{
    public abstract class IUIScreen : MonoBehaviour, System.IDisposable
    {
        protected IRGNFrame _rgnFrame;

        public virtual Task InitAsync(IRGNFrame rgnFrame)
        {
            _rgnFrame = rgnFrame;
            return Task.CompletedTask;
        }
        protected abstract void Dispose(bool disposing);

        public void Dispose()
        {
            Dispose(disposing: true);
        }

        public virtual void SetVisible(bool visible)
        {
            gameObject.SetActive(visible);
        }
        protected void OnBackButtonClick()
        {
            _rgnFrame.CloseScreen(GetType());
        }
    }
}
