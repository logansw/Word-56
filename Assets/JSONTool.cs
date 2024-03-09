using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class JSONTool
{
    public static void WriteData<JSONData>(JSONData data, string fileName) {
        string savePath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + fileName;
        string json = JsonUtility.ToJson(data);
        using StreamWriter writer = new StreamWriter(savePath);
        writer.Write(json);
    }

    public static T ReadData<T>(string fileName) where T : IJSONData<T>, new(){
        if (!FileExists(fileName)) {
            T tData = new T();
            tData = tData.CreateNewFile();
            JSONTool.WriteData(tData, fileName);
        }
        using StreamReader reader = new StreamReader(Application.persistentDataPath + Path.AltDirectorySeparatorChar + fileName);
        string json = reader.ReadToEnd();
        T data = JsonUtility.FromJson<T>(json);
        return data;
    }

    public static bool FileExists(string fileName) {
        return File.Exists(Application.persistentDataPath + Path.AltDirectorySeparatorChar + fileName);
    }
}