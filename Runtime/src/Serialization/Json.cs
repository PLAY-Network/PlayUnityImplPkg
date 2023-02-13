using System.IO;
using Newtonsoft.Json;
using RGN.Dependencies.Serialization;
using UnityEngine;

namespace RGN.Impl.Firebase.Serialization
{
    public sealed class Json : IJson
    {
        private readonly JsonSerializer mSerializer = new JsonSerializer();

        T IJson.FromJson<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        object IJson.FromJson(string json, System.Type type)
        {
            return JsonConvert.DeserializeObject(json, type);
        }

        void IJson.FromJsonOverwrite(string json, object objectToOverwrite)
        {
            JsonUtility.FromJsonOverwrite(json, objectToOverwrite);
        }

        string IJson.ToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        string IJson.ToJson(object obj, bool prettyPrint)
        {
            return JsonUtility.ToJson(obj, prettyPrint);
        }

        public T FromJson<T>(Stream stream)
        {
            using (var streamReader = new StreamReader(stream))
            {
                using (var jsonTextReader = new JsonTextReader(streamReader))
                {
                    return mSerializer.Deserialize<T>(jsonTextReader);
                }
            }
        }
    }
}
