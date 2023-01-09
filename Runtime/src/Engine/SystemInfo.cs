using RGN.Dependencies.Engine;

namespace RGN.Impl.Firebase.Engine
{
    public sealed class SystemInfo : ISystemInfo
    {
        public string ProcessorType => UnityEngine.SystemInfo.processorType;

        public int ProcessorCount => UnityEngine.SystemInfo.processorCount;

        public int SystemMemorySize => UnityEngine.SystemInfo.systemMemorySize;

        public string GraphicsDeviceName => UnityEngine.SystemInfo.graphicsDeviceName;

        public int GraphicsMemorySize => UnityEngine.SystemInfo.graphicsMemorySize;
    }
}
