using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  Autor do Script:            Ricarth Lima
 *  Data Script:                01/04/2021
 *  Versão do Script:           1.0
 *  Finalidade do Script:       Controlador responsável por gerenciar todos os sons do jogo.
 */

public class SoundController : MonoBehaviour
{
    [SerializeField]
    private GameObject soundBGM, soundClickButton, soundScored, soundBite, soundGameOver, soundMotor;

    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void SetActiveBGM(bool value)
    {
        soundBGM.SetActive(value);
    }

    public void ActiveButtonClick()
    {
        Instantiate(this.soundClickButton);
    }

    public void PlayScoredSound()
    {
        Instantiate(this.soundScored);
    }

    public void PlayBiteSound()
    {
        Instantiate(this.soundBite);
    }

    public void PlayGameOver()
    {
        Instantiate(this.soundGameOver);
    }

    public void PlayMotorSound()
    {
        this.soundMotor.SetActive(true);
    }

    public void StopMotorSound()
    {
        this.soundMotor.SetActive(false);
    }
}