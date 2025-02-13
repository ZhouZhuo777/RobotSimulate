using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public static class FileTools
{
    public static void WriteTextFile(string path, string data)
    {
        string savePath = Path.GetDirectoryName(path);
        if (!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);
        
        File.WriteAllText(path,data);
    }

    public static void WriteBytesFile(string path, byte[] data)
    {
        string savePath = Path.GetDirectoryName(path);
        if (!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);
        
        File.WriteAllBytes(path,data);
    }

    public static string ReadTextFile(string path)
    {
        return File.ReadAllText(path, Encoding.UTF8);
    }

    public static byte[] ReadBytesFile(string path)
    {
        return File.ReadAllBytes(path);
    }

    public static void DeleteFile(string path)
    {
        if (File.Exists(path))
        {
            try
            {
                File.Delete(path);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        else
        {
            Debug.LogError("Delete file fail is not exists : "+path);
        }
    }

    
    /// <summary>
    /// 递归获取目标路径下的所有文件
    /// </summary>
    /// <param name="path">路径必须是文件夹的路径</param>
    /// <returns></returns>
    public static List<string> GetAllFile(string path)
    {
        if (!Directory.Exists(path))
        {
            Debug.LogError("不存在的文件夹：" + path);
            return null;
        }

        List<string> list = new List<string>();
        list.AddRange(Directory.GetFiles(path));
        foreach (var item in GetAllDirectory(path))
        {
            list.AddRange(Directory.GetFiles(item));
        }

        return list;
    }

    /// <summary>
    /// 递归获取所有目标文件下的所有文件夹
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static List<string> GetAllDirectory(string path)
    {
        List<string> list = new List<string>();
        GetChildDirectory(path, ref list);
        return list;
    }

    /// <summary>
    /// 获取该文件夹下的文件夹
    /// </summary>
    /// <param name="path"></param>
    /// <param name="directoryPaths"></param>
    private static void GetChildDirectory(string path, ref List<string> directoryPaths)
    {
        if (Directory.Exists(path))
        {
            foreach (var item in Directory.GetDirectories(path))
            {
                directoryPaths.Add(item);
                GetChildDirectory(item, ref directoryPaths);
            }
        }
    }

    /// <summary>
    /// 获取该文件夹下的文件夹
    /// </summary>
    /// <param name="path"></param>
    /// <param name="directoryPaths"></param>
    public static List<string> GetChildDirectory(string path)
    {
        List<string> directoryPaths = new List<string>();
        if (Directory.Exists(path))
        {
            directoryPaths.AddRange(Directory.GetDirectories(path));
        }

        return directoryPaths;
    }
    
    public static void CopyFolder(string strFromPath,string strToPath)
    {
        var str=Directory.GetParent(strFromPath);
        foreach (var item in GetAllFile(strFromPath))
        {
            Debug.Log(str.FullName);
            var newPath= strToPath+item.Remove(0,str.FullName.Length);
            Debug.Log(newPath);

           FileTools.WriteBytesFile(newPath,File.ReadAllBytes(item));
        }
    }
}