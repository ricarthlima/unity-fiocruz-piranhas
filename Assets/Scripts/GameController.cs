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

    [SerializeField]
    private GameObject canvaMainMenu, canvaInGame;

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

    public Sprite[] listLifesSprites;
    public Image imgLifesDisplay;

    private void Awake()
    {
        RefreshInfos();
    }

    private void Start()
    {
        this.backgroundController = gameObject.GetComponent<BackgroundController>();
        this.soundController = gameObject.GetComponent<SoundController>();
        this.spawnController = gameObject.GetComponent<SpawnController>();
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
        this.soundController.ActiveButtonClick();
        this.isGameStarted = true;
        canvaMainMenu.SetActive(false);
        canvaInGame.SetActive(true);
        backgroundController.setActivePiranhaSwimming(false);
        this.spawnController.StartSpawn();

        this.soundController.PlayMotorSound();

        InvokeRepeating("NextLevel", 25f, 25f);
    }

    public void ShowRanking()
    {
        this.soundController.PlayBiteSound();
    }

    public void GameOver()
    {
        if (this.points > this.highScore)
        {
            PlayerPrefs.SetInt(PrefsKeys.highScore, this.points);
        }

        this.soundController.PlayGameOver();

        playerLifes = 3;
        points = 0;
        level = 0;

        this.isGameStarted = false;
        canvaMainMenu.SetActive(true);
        canvaInGame.SetActive(false);
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

    private void NextLevel()
    {
        this.level += 1;

        GameObject[] arrayEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in arrayEnemies)
        {
            StartCoroutine("FleePiranhas", enemy);
        }
    }

    private IEnumerator FleePiranhas(GameObject enemy)
    {
        yield return new WaitForSeconds(0.15f);
        MakeAPoint(7);
        Destroy(enemy);
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
}