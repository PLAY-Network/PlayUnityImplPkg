using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace RGN.Impl.Firebase
{
    public interface IRGNFrame
    {
        void OpenScreen<TScreen>();
        void CloseScreen<TScreen>();
        void CloseScreen(System.Type type);
    }

    public class RGNFrame : RGNUnityInitializer, IRGNFrame
    {
        [SerializeField] private IUIScreen[] _initializables;

        private readonly Dictionary<System.Type, IUIScreen> mRegisteredScreens =
            new Dictionary<System.Type, IUIScreen>();
        private readonly Stack<IUIScreen> mScreensStack = new Stack<IUIScreen>();

        private IUIScreen _currentVisibleScreen;

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();
            RGNCore.I.UpdateEvent += OnUpdate;

            for (int i = 0; i < _initializables.Length; ++i)
            {
                var screen = _initializables[i];
                if (i == 0)
                {
                    screen.SetVisible(true);
                    _currentVisibleScreen = screen;
                }
                else
                {
                    screen.SetVisible(false);
                }
            }
            for (int i = 0; i < _initializables.Length; ++i)
            {
                var screen = _initializables[i];
                await screen.InitAsync(this);
                mRegisteredScreens.Add(screen.GetType(), screen);
            }
        }
        protected override void Dispose(bool disposing)
        {
            for (int i = 0; i < _initializables.Length; ++i)
            {
                _initializables[i].Dispose();
            }
            RGNCore.I.UpdateEvent -= OnUpdate;
            base.Dispose(disposing);
        }
        private void OnUpdate()
        {
            if (Input.GetKeyUp(KeyCode.Escape) && _currentVisibleScreen != null)
            {
                CloseScreen(_currentVisibleScreen.GetType());
            }
        }

        public void OpenScreen<TScreen>()
        {
            var screenTypeToOpen = typeof(TScreen);
            if (mRegisteredScreens.TryGetValue(screenTypeToOpen, out var screen))
            {
                if (_currentVisibleScreen != null)
                {
                    mScreensStack.Push(_currentVisibleScreen);
                    _currentVisibleScreen.SetVisible(false);
                }
                screen.SetVisible(true);
                _currentVisibleScreen = screen;
                return;
            }
            Debug.LogError("Can not find screen to open: " + screenTypeToOpen);
        }
        public void CloseScreen<TScreen>()
        {
            var screenTypeToClose = typeof(TScreen);
            CloseScreen(screenTypeToClose);
        }
        public void CloseScreen(System.Type screenTypeToClose)
        {
            if (mRegisteredScreens.TryGetValue(screenTypeToClose, out var screen))
            {
                screen.SetVisible(false);
                _currentVisibleScreen = null;
                if (mScreensStack.Count > 0)
                {
                    _currentVisibleScreen = mScreensStack.Pop();
                    _currentVisibleScreen.SetVisible(true);
                }
                return;
            }
            Debug.LogError("Can not find screen to close: " + screenTypeToClose);
        }
    }
}
