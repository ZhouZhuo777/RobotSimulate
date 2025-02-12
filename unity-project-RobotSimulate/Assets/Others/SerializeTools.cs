using System;
using Newtonsoft.Json;
using UnityEngine;

public static class SerializeTools
{

    public static string ObjToString<T>(T obj, Formatting indented = Formatting.Indented)
    {
        return JsonConvert.SerializeObject(obj, indented);
    }
    public static T StringToObj<T>(string str)
    {
        if (!str.IsNotNullOrEmpty()) throw new NullReferenceException("JsonConvert DeserializeObject is null");
        T result = JsonConvert.DeserializeObject<T>(str);
        
        if (result==null) throw new NullReferenceException($"JsonConvert DeserializeObject is null \nDeserialize string is : {str}");
        return result;
    }
    
    public static string JsonConvertObjToString1<T>(T obj, Formatting indented = Formatting.Indented)
    {
        return JsonConvert.SerializeObject(obj, indented);
    }
    

    public static T JsonConvertStringToObj1<T>(string str)
    {
        if (!str.IsNotNullOrEmpty()) throw new NullReferenceException("JsonConvert DeserializeObject is null");
        T result = JsonConvert.DeserializeObject<T>(str);
        if (result==null) throw new NullReferenceException($"JsonConvert DeserializeObject is null \nDeserialize string is : {str}");
        return result;
    }
    
    public static string JsonUtilityObjToString<T>(T obj, Formatting indented = Formatting.Indented)
    {
        return JsonUtility.ToJson(obj);
    }
    public static T JsonUtilityStringToObj<T>(string str)
    {
        T result =JsonUtility.FromJson<T>(str);
        if (result==null) throw new NullReferenceException($"JsonUtilityStringToObj DeserializeObject is null \nDeserialize string is : {str}");
        return result;
    }

    #region 可以序列化存储具体的类型
    public static string JsonConvertObjToString2<T>(T obj, Formatting indented = Formatting.Indented)
    {
        return JsonConvert.SerializeObject(obj, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        });
    }
    public static T JsonConvertStringToObj2<T>(string str)
    {
        if (!str.IsNotNullOrEmpty()) throw new NullReferenceException("JsonConvert DeserializeObject is null");
        T result = JsonConvert.DeserializeObject<T>(str,new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        });
        if (result==null) throw new NullReferenceException($"JsonConvert DeserializeObject is null \nDeserialize string is : {str}");
        return result;
    }
    #endregion
}