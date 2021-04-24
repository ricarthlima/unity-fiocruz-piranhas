using UnityEngine;
using Firebase;
using System;
using Firebase.Database;

using UnityEngine.UI;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine.Networking;
using System.Collections;
using MiniJSON;
using Newtonsoft.Json.Linq;

public class FirebaseController : MonoBehaviour
{
    public Text txtRanking;
    public Text txtPoints;

    private bool isMostra = false;
    private string saveRanking = "";
    private string savePoints = "";

    private void Start()
    {
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

    public string GetUuid()
    {
        string value = PlayerPrefs.GetString(PrefsKeys.uuid);
        if (value == null || value == "")
        {
            value = "NONE";
            PlayerPrefs.SetString(PrefsKeys.uuid, value);
        }
        return value;
    }

    public void UpdateRankingScreen()
    {
        StartCoroutine(UpdateData());
    }

    private IEnumerator UpdateData()
    {
        String stringUsernames = "";
        String stringPoints = "";

        UnityWebRequest request = UnityWebRequest.Get("https://piranha-attack-api.herokuapp.com/top10/" + GetUuid());
        request.SetRequestHeader("Authorization", Secrets.SECRET_TOKEN_API);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            Debug.Log(json);

            Dictionary<string, object> dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

            int playerPosition = -1;
            if (dict["player_position"] != null)
            {
                playerPosition = int.Parse(dict["player_position"].ToString());
            }

            string microJson = dict["ranking"].ToString();
            List<DTOScoreModel> list = JsonConvert.DeserializeObject<List<DTOScoreModel>>(microJson);

            for (int i = 0; i < list.Count; i++)
            {
                DTOScoreModel element = list[i];
                stringUsernames = stringUsernames + (i + 1).ToString() + ". " + element.username + "\n";
                stringPoints = stringPoints + element.score + "\n";
            }
            this.saveRanking = stringUsernames;
            this.savePoints = stringPoints;
            isMostra = true;
        }
    }
}

[Serializable]
public class DTOScoreModel
{
    public Dictionary<string, object> _id;
    public int score;
    public string username;
}