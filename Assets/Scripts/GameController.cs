using DragonBones;
using FMOD;
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

    [SerializeField]
    private List<Sprite> listSprite;

    private BackgroundController backgroundController;
    private SoundController soundController;
    private SpawnController spawnController;

    [SerializeField]
    private GameObject canvaMainMenu, canvaInGame;

    [Header("In-game Controller")]
    public int playerLifes = 3;     // Quantidade de vidas disponíveis

    public int points = 0;          // Total de pontos obtidos (10 pontos por piranha afastada)
    public int level = 0;

    [Header("UI Objects")]
    public Text txtScore;

    public Sprite[] listLifesSprites;
    public Image imgLifesDisplay;

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
            btnSoundController.GetComponent<Image>().sprite = listSprite[1];
        }
        else
        {
            this.soundController.SetActiveBGM(true);
            btnSoundController.GetComponent<Image>().sprite = listSprite[0];
        }
        isBGMActive = !isBGMActive;
    }

    public void gameStart()
    {
        this.imgLifesDisplay.sprite = this.listLifesSprites[playerLifes];
        this.soundController.ActiveButtonClick();
        this.isGameStarted = true;
        canvaMainMenu.SetActive(false);
        canvaInGame.SetActive(true);
        backgroundController.setActivePiranhaSwimming(false);
        this.spawnController.StartSpawn();
    }

    public void ShowRanking()
    {
        this.soundController.PlayBiteSound();
    }

    public void GameOver()
    {
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
    }

    public void MakeAPoint(int point)
    {
        this.soundController.PlayScoredSound();
        this.points += point;
        txtScore.text = points.ToString();
    }

    public void TakeADamage()
    {
        Invoke("DelaySoundBite", 1.4f);
    }

    private void DelaySoundBite()
    {
        this.soundController.PlayBiteSound();
        playerLifes -= 1;
        this.imgLifesDisplay.sprite = this.listLifesSprites[playerLifes];

        if (playerLifes <= 0)
        {
            GameOver();
        }
    }
}