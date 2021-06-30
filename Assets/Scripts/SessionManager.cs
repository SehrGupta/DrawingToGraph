using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class SessionManager : MonoBehaviour
{
    private static string _path
    {
        get
        {
            if (Application.platform == RuntimePlatform.WindowsEditor)
                return Application.dataPath + "/Saves/";
            else
                return Application.persistentDataPath + "/Saves/";
        }
    }

    public static SessionManager Instance { get; private set; }

    static public Dictionary<int, JsonScene> CreatedScenes { get; private set; }
    static public int CurrentLevel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            CreatedScenes = new Dictionary<int, JsonScene>();
        }
        else if (Instance != this) { Destroy(this); }
    }

    public static void AddScene(JsonScene scene)
    {
        if (CreatedScenes.ContainsKey(CurrentLevel)) CreatedScenes[CurrentLevel] = scene;
  
        else CreatedScenes.Add(CurrentLevel,scene);
    }


    // Attach this to a button that represents the user is done
    public static void SaveScenes()
    {
        foreach (var level in CreatedScenes.Keys)
        {
            JsonScene jsonScene = CreatedScenes[level];
            string json = JsonUtility.ToJson(jsonScene);
            string filename = $"Level{level}.json";

            try
            {
                if (!Directory.Exists(_path)) Directory.CreateDirectory(_path);                                           // changed in technical tutorial

                System.IO.File.WriteAllText(_path + filename, json);
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
            }
        }
    }
}
