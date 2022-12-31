using ReadyGamesNetwork.RGN.Dependencies.Engine;
using RGN.Dependencies.Engine;
using RGN.Utility;
using UnityEngine;

namespace RGN.Impl.Firebase.Engine
{
    public sealed class EngineApp : IEngineApp
    {
        bool IEngineApp.IsStandalonePlatform =>
            Application.platform == RuntimePlatform.OSXPlayer ||
            Application.platform == RuntimePlatform.WindowsPlayer ||
            Application.platform == RuntimePlatform.LinuxPlayer;

        bool IEngineApp.IsEditor =>
            Application.platform == RuntimePlatform.OSXEditor ||
            Application.platform == RuntimePlatform.WindowsEditor ||
            Application.platform == RuntimePlatform.LinuxEditor;

        IRGNUpdater IEngineApp.CreateGameObjectWithUpdater()
        {
            GameObject go = new GameObject("RGNModuleCore");
            Object.DontDestroyOnLoad(go);
            RGNUnityUpdater updater = go.AddComponent<RGNUnityUpdater>();
            return updater;
        }
        public void DestroyGameObjectWithUpdater(IRGNUpdater rgnUpdater)
        {
            var asComponent = rgnUpdater as RGNUnityUpdater;
            Object.Destroy(asComponent?.gameObject);
        }

        ITexture2D IEngineApp.CreateTexture2D(int width, int height)
        {
            UnityEngine.Texture2D texture2D = new UnityEngine.Texture2D(200, 200);
            ITexture2D texture = new Texture2D(texture2D);
            return texture;
        }
    }
}
