using UnityEngine;
using UnityEngine.UI;

namespace RGN.Samples
{
    public sealed class AuthSample : MonoBehaviour, System.IDisposable
    {
        [SerializeField] private Button _helloWorld;

        private void Awake()
        {
            _helloWorld.onClick.AddListener(OnHelloWorldButtonClick);
        }
        public void Dispose()
        {
            _helloWorld.onClick.RemoveListener(OnHelloWorldButtonClick);
        }

        private void OnHelloWorldButtonClick()
        {
            Debug.Log("OnHelloWorldButtonClick");
        }
    }
}
