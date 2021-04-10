using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using System;
using Firebase.Database;
using MiniJSON;

public class FirebaseController : MonoBehaviour
{
    private FirebaseApp app;
    private DatabaseReference mDatabaseRef;

    private void Start()
    {
        FirebaseApp.DefaultInstance.Options.DatabaseUrl = new Uri("https://piranha-attack-ef368-default-rtdb.firebaseio.com/");
        mDatabaseRef = FirebaseDatabase.DefaultInstance.RootReference;
        InitializeBD();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void InitializeBD()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                app = FirebaseApp.DefaultInstance;
                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
            }
        });
    }

    public void RecordData(string username, int points)
    {
        RankingModel model = new RankingModel(username, points);
        string json = JsonUtility.ToJson(model);
        string uuid = GetUuid();
        mDatabaseRef.Child("ranking").Child(uuid).SetRawJsonValueAsync(json);
    }

    public string GetUuid()
    {
        string value = PlayerPrefs.GetString(PrefsKeys.uuid);
        if (value == null || value == "")
        {
            value = System.Guid.NewGuid().ToString();
            PlayerPrefs.SetString(PrefsKeys.uuid, value);
        }
        return value;
    }
}

public class RankingModel
{
    public String username;
    public int points;

    public RankingModel(String username, int points)
    {
        this.username = username;
        this.points = points;
    }
}