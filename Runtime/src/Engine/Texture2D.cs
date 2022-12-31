using RGN.Dependencies.Engine;
using UnityEngine;

namespace RGN.Impl.Firebase.Engine
{
    public sealed class Texture2D : ITexture2D
    {
        private readonly UnityEngine.Texture2D texture;

        public Texture2D(UnityEngine.Texture2D texture)
        {
            this.texture = texture;
        }
        public void Dispose()
        {
            Object.DestroyImmediate(texture);
        }

        byte[] ITexture2D.EncodeToPNG()
        {
            return texture.EncodeToPNG();
        }
        bool ITexture2D.LoadImage(byte[] data)
        {
            return texture.LoadImage(data);
        }
        object ITexture2D.GetWrappedTexture()
        {
            return texture;
        }
    }
}
