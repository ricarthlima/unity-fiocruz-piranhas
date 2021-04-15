using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public bool isGameStarted;
    public bool isBGMActive;

    [Header("Button Sound")]
    [SerializeField]
    private Button btnSoundController;

    private BackgroundController backgroundController;
    private SoundController soundController;
    private SpawnController spawnController;
    private FirebaseController firebaseController;

    [SerializeField]
    private GameObject canvaMainMenu, canvaInGame, canvaNewRecord;

    [Header("In-game Controller")]
    public int playerLifes = 3;     // Quantidade de vidas disponíveis

    public int points = 0;          // Total de pontos obtidos (10 pontos por piranha afastada)
    public int level = 1;

    public int highScore = 0;

    [Header("UI Objects - MainMenu")]
    public List<Sprite> listSpeakerSprites;

    public Text txtHighScore;

    [Header("UI Objects - In Game")]
    public Text txtScore;

    public Text txtLevels;

    public Sprite[] listLifesSprites;
    public Image imgLifesDisplay;

    [Header("UI Objects - New Record")]
    public InputField inputUsername;

    [Header("UI Objects - Ranking")]
    public GameObject canvaRanking;

    public Text txtRanking;
    public Text txtPoints;

    private void Awake()
    {
        RefreshInfos();
    }

    private void Start()
    {
        this.backgroundController = gameObject.GetComponent<BackgroundController>();
        this.soundController = gameObject.GetComponent<SoundController>();
        this.spawnController = gameObject.GetComponent<SpawnController>();
        this.firebaseController = gameObject.GetComponent<FirebaseController>();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void changeActiveBGM()
    {
        this.soundController.ActiveButtonClick();
        if (isBGMActive)
        {
            this.soundController.SetActiveBGM(false);
            btnSoundController.GetComponent<Image>().sprite = listSpeakerSprites[1];
        }
        else
        {
            this.soundController.SetActiveBGM(true);
            btnSoundController.GetComponent<Image>().sprite = listSpeakerSprites[0];
        }
        isBGMActive = !isBGMActive;
    }

    public void gameStart()
    {
        this.imgLifesDisplay.sprite = this.listLifesSprites[playerLifes];
        txtScore.text = points.ToString();
        this.txtLevels.text = "Level: " + this.level.ToString();

        this.soundController.ActiveButtonClick();
        this.isGameStarted = true;
        canvaMainMenu.SetActive(false);
        canvaInGame.SetActive(true);
        backgroundController.setActivePiranhaSwimming(false);
        this.spawnController.StartSpawn();

        this.soundController.PlayMotorSound();

        StartCoroutine("NextLevel");
    }

    public void ShowRanking()
    {
        this.soundController.PlayBiteSound();
        this.ShowRankingScreen();
    }

    public void GameOver()
    {
        StopCoroutine("NextLevel");
        this.isGameStarted = false;
        canvaInGame.SetActive(false);

        if (this.points > this.highScore)
        {
            PlayerPrefs.SetInt(PrefsKeys.highScore, this.points);
            canvaNewRecord.SetActive(true);
        }
        else
        {
            canvaMainMenu.SetActive(true);
        }

        this.soundController.PlayGameOver();
        points = 0;
        playerLifes = 3;
        level = 0;

        this.spawnController.StopSpawn();
        backgroundController.setActivePiranhaSwimming(true);

        GameObject[] arrayEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in arrayEnemies)
        {
            Destroy(enemy);
        }

        this.soundController.StopMotorSound();
        this.RefreshInfos();
    }

    public void ButtonSendNewRecord()
    {
        string text = this.inputUsername.text;
        if (text.Length >= 3 && text.Length <= 20)
        {
            int hs = PlayerPrefs.GetInt(PrefsKeys.highScore);
            this.firebaseController.RecordData(text, hs);
            this.canvaNewRecord.SetActive(false);
            this.canvaMainMenu.SetActive(true);
        }
    }

    public void MakeAPoint(int point)
    {
        this.soundController.PlayScoredSound();
        this.points += point;
        txtScore.text = points.ToString();
    }

    public void TakeADamage()
    {
        this.soundController.PlayBiteSound();
        playerLifes -= 1;
        this.imgLifesDisplay.sprite = this.listLifesSprites[playerLifes];

        if (playerLifes <= 0)
        {
            GameOver();
        }
    }

    private IEnumerator NextLevel()
    {
        yield return new WaitForSeconds(25f);

        StartCoroutine("FleePiranhas");

        this.level += 1;

        this.txtLevels.text = "Level: " + this.level.ToString();

        StartCoroutine("NextLevel");
    }

    private IEnumerator FleePiranhas()
    {
        yield return new WaitForSeconds(0.10f);
        GameObject[] arrayEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (arrayEnemies.Length > 0)
        {
            MakeAPoint(7);
            Destroy(arrayEnemies[0]);
            StartCoroutine("FleePiranhas");
        }
        else
        {
            StopCoroutine("FleePiranhas");
        }
    }

    private void RefreshInfos()
    {
        this.highScore = PlayerPrefs.GetInt(PrefsKeys.highScore);
        txtHighScore.text = "REC: " + this.highScore.ToString();
    }

    public void TooglePauseGame()
    {
        this.soundController.ActiveButtonClick();
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
        }
        else if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
    }

    public void BackToMainScreenFromRankingScreen()
    {
        this.soundController.ActiveButtonClick();
        this.canvaRanking.SetActive(false);
        this.canvaMainMenu.SetActive(true);
    }

    public void ShowRankingScreen()
    {
        this.canvaRanking.SetActive(true);
        this.canvaMainMenu.SetActive(false);
        this.canvaInGame.SetActive(false);
        this.canvaNewRecord.SetActive(false);
        this.firebaseController.UpdateRankingScreen();
    }
}