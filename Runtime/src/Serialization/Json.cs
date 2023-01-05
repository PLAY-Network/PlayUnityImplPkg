using RGN.Dependencies.Serialization;
using UnityEngine;

namespace RGN.Impl.Firebase.Serialization
{
    public sealed class Json : IJson
    {
        T IJson.FromJson<T>(string json)
        {
            return JsonUtility.FromJson<T>(json);
        }

        object IJson.FromJson(string json, System.Type type)
        {
            return JsonUtility.FromJson(json, type);
        }

        void IJson.FromJsonOverwrite(string json, object objectToOverwrite)
        {
            JsonUtility.FromJsonOverwrite(json, objectToOverwrite);
        }

        string IJson.ToJson(object obj)
        {
            return JsonUtility.ToJson(obj);
        }

        string IJson.ToJson(object obj, bool prettyPrint)
        {
            return JsonUtility.ToJson(obj, prettyPrint);
        }
    }
}
