using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    private GameController gameContoller;

    private void Start()
    {
        GameObject gameControllerObject = GameObject.Find("GameControllerObject");
        this.gameContoller = gameControllerObject.GetComponent<GameController>();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void btnStartPressed()
    {
        gameObject.SetActive(false);
        this.gameContoller.isGameStarted = true;
    }
}