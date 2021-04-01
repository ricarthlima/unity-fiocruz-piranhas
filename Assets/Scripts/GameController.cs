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
    public int playerLifes;     // Quantidade de vidas dispon�veis

    public int points;          // Total de pontos obtidos (10 pontos por piranha afastada)
    public int level;

    [Header("UI Objects")]
    public Text txtScore;

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
        this.soundController.ActiveButtonClick();
        this.isGameStarted = true;
        canvaMainMenu.SetActive(false);
        canvaInGame.SetActive(true);
        backgroundController.gameStarted();
        this.spawnController.StartSpawn();
    }

    public void MakeAPoint(int point)
    {
        this.points += point;
        txtScore.text = points.ToString();
    }
}