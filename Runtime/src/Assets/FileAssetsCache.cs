using System;
using System.IO;
using RGN.ImplDependencies.Assets;
using UnityEngine;

namespace RGN.Impl.Firebase.Assets
{
    public class FileAssetsCache : IAssetCache
    {
        private string BasePath => Application.persistentDataPath;

        public bool HasInCache(AssetCategory category, string key)
        {
            CreateCategoryDirectoryIfThereNo(category);
            
            string assetPath = GetAssetPath(category, key);
            return File.Exists(assetPath);
        }

        public byte[] ReadFromCache(AssetCategory category, string key)
        {
            CreateCategoryDirectoryIfThereNo(category);
            
            string assetPath = GetAssetPath(category, key);
            byte[] assetBytes = File.ReadAllBytes(assetPath);
            return assetBytes;
        }

        public bool TryReadFromCache(AssetCategory category, string key, out byte[] data)
        {
            CreateCategoryDirectoryIfThereNo(category);

            if (!HasInCache(category, key))
            {
                data = Array.Empty<byte>();
                return false;
            }

            data = ReadFromCache(category, key);
            return true;
        }

        public void WriteToCache(AssetCategory category, string key, byte[] data)
        {
            CreateCategoryDirectoryIfThereNo(category);

            string assetPath = GetAssetPath(category, key);
            File.WriteAllBytes(assetPath, data);
        }

        public void Clear()
        {
            foreach (AssetCategory category in (AssetCategory[])System.Enum.GetValues(typeof(AssetCategory)))
            {
                ClearCategoryDirectory(category);
            }
        }

        private void CreateCategoryDirectoryIfThereNo(AssetCategory category)
        {
            string categoryPath = Path.Combine(BasePath, category.ToString());
            
            if (!Directory.Exists(categoryPath))
            {
                Directory.CreateDirectory(categoryPath);
            }
        }
        
        private void ClearCategoryDirectory(AssetCategory category)
        {
            string categoryPath = GetCategoryPath(category);
            
            if (Directory.Exists(categoryPath))
            {
                DirectoryInfo assetsDirectory = new DirectoryInfo(categoryPath);
                foreach (FileInfo assetFile in assetsDirectory.GetFiles())
                {
                    assetFile.Delete();
                }
            }
        }

        private string GetCategoryPath(AssetCategory category)
            => Path.Combine(BasePath, category.ToString());
        
        private string GetAssetPath(AssetCategory category, string key)
            => Path.Combine(GetCategoryPath(category), key);
    }
}
