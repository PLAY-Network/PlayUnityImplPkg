using System.Threading.Tasks;
using UnityEngine;

namespace RGN.Impl.Firebase
{
    public abstract class IInitializable : MonoBehaviour, System.IDisposable
    {
        public abstract Task InitAsync();
        protected abstract void Dispose(bool disposing);

        public void Dispose()
        {
            Dispose(disposing: true);
        }
    }
}
