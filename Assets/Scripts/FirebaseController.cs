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
using FMOD;
using System.Runtime.Serialization.Json;

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

    public void SetUuid(string uuid)
    {
        PlayerPrefs.SetString(PrefsKeys.uuid, uuid);
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

    public void RecordData(string text, int hs)
    {
        StartCoroutine(InternalRecordData(text, hs));
    }

    private IEnumerator InternalRecordData(string text, int hs)
    {
        print(GetUuid());
        if (GetUuid() != "NONE")
        {
            UnityWebRequest deleteRequest = UnityWebRequest.Delete("https://piranha-attack-api.herokuapp.com/score_registers/" + GetUuid());
            deleteRequest.SetRequestHeader("Authorization", Secrets.SECRET_TOKEN_API);
            yield return deleteRequest.SendWebRequest();
        }
        DTOPostModel model = new DTOPostModel(text, hs);
        //UnityWebRequest request = UnityWebRequest.Post("https://piranha-attack-api.herokuapp.com/score_registers", a);
        UnityWebRequest request = new UnityWebRequest("https://piranha-attack-api.herokuapp.com/score_registers");
        request.method = "POST";
        request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(model)));
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Authorization", Secrets.SECRET_TOKEN_API);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            DTOScoreModel model2 = JsonConvert.DeserializeObject<DTOScoreModel>(request.downloadHandler.text);
            print(model2.getUUID());
            SetUuid(model2.getUUID());
        }
        else
        {
            print(request.error);
        }
    }
}

[Serializable]
public class DTOScoreModel
{
    public Dictionary<string, object> _id;
    public int score;
    public string username;

    public string getUUID()
    {
        return _id["$oid"].ToString();
    }
}

[Serializable]
public class DTOPostModel
{
    public int score;
    public string username;

    public DTOPostModel(string username, int score)
    {
        this.username = username;
        this.score = score;
    }

    public Dictionary<string, string> ToPayload()
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();
        dict["username"] = this.username;
        dict["score"] = this.score.ToString();
        return dict;
    }

    public string ToJson()
    {
        return "{'username': '" + this.username + "','score': " + this.score.ToString() + "}";
    }
}