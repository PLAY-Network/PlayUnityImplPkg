using UnityEngine;

namespace RGN.Impl.Firebase
{
    public class RGNUnityInitializer : MonoBehaviour
    {
        private bool _initialized = false;
        private async void Awake()
        {
            if (_initialized)
            {
                return;
            }
            await RGNCoreBuilder.BuildAsync(new Dependencies());
            _initialized = true;
        }
    }
}
