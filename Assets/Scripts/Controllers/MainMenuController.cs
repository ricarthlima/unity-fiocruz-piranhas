using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    private GameController gameContoller;

    [SerializeField]
    private GameObject gameControllerObject, canvasMainMenu, soundControllerButton;

    private void Start()
    {
        this.gameContoller = gameControllerObject.GetComponent<GameController>();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void BtnStartPressed()
    {
        canvasMainMenu.SetActive(false);
        this.gameContoller.isGameStarted = true;
        EnableSoundController();
    }

    public void BtnShowRecordsPressed()
    {
        EnableSoundController();
    }

    private void EnableSoundController()
    {
        this.soundControllerButton.SetActive(true);
        Invoke("DesableSoundController", 0.7f);
    }

    private void DesableSoundController()
    {
        this.soundControllerButton.SetActive(false);
    }
}