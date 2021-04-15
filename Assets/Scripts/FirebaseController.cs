using UnityEngine;
using Firebase;
using System;
using Firebase.Database;

using UnityEngine.UI;
using System.Collections.Generic;
using Newtonsoft.Json;

public class FirebaseController : MonoBehaviour
{
    private FirebaseApp app;
    private DatabaseReference mDatabaseRef;

    public Text txtRanking;
    public Text txtPoints;

    private bool isMostra = false;
    private string saveRanking = "";
    private string savePoints = "";

    private void Start()
    {
        FirebaseApp.DefaultInstance.Options.DatabaseUrl = new Uri("https://piranha-attack-ef368-default-rtdb.firebaseio.com/");
        mDatabaseRef = FirebaseDatabase.DefaultInstance.RootReference;
        InitializeBD();
    }

    // Update is called once per frame
    private void Update()
    {
        if (isMostra)
        {
            txtRanking.text = saveRanking;
            txtPoints.text = savePoints;
            isMostra = false;
        }
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

    public void UpdateRankingScreen()
    {
        FirebaseDatabase.DefaultInstance
       .GetReference("ranking")
       .GetValueAsync().ContinueWith(task =>
       {
           if (task.IsFaulted)
           {
               // Handle the error...
           }
           else if (task.IsCompleted)
           {
               DataSnapshot snapshot = task.Result;
               string json = snapshot.GetRawJsonValue();
               //print("JSON:\n" + json);

               Dictionary<string, RankingModel> dict = JsonConvert.DeserializeObject<Dictionary<string, RankingModel>>(json);
               List<RankingModelID> list = new List<RankingModelID>();

               foreach (string key in dict.Keys)
               {
                   RankingModelID withId = new RankingModelID(key, dict[key].username, dict[key].points);
                   list.Add(withId);
               }

               list.Sort((x, y) => y.points.CompareTo(x.points));

               String stringUsernames = "";
               String stringPoints = "";

               for (int i = 0; i < Math.Min(10, list.Count); i++)
               {
                   RankingModelID model = list[i];
                   stringUsernames = stringUsernames + (i + 1).ToString() + ". " + model.username + "\n";
                   stringPoints = stringPoints + model.points + "\n";
               }

               //bool found = false;
               //int j = 0;
               //string myuuid = Getuuid();
               //while (j < list.count && !found)
               //{
               //    if (list[j].id == myuuid)
               //    {
               //        found = true;
               //    }
               //    j++;
               //}

               //if (found)
               //{
               //    stringusernames = stringusernames + "\n-----------\n" + (j + 1).tostring() + ". " + list[j].username;
               //    stringpoints = stringpoints + "\n-----------\n" + list[j].points;
               //}

               //print(stringUsernames);

               this.saveRanking = stringUsernames;
               this.savePoints = stringPoints;
               this.isMostra = true;
           }
       });
    }
}

[Serializable]
public class RankingModel
{
    public string username;
    public int points;

    public RankingModel(string username, int points)
    {
        this.username = username;
        this.points = points;
    }
}

[Serializable]
public class RankingModelID
{
    public string id;
    public string username;
    public int points;

    public RankingModelID(string id, string username, int points)
    {
        this.id = id;
        this.username = username;
        this.points = points;
    }
}