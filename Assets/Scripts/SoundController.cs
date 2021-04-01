using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [Header("Sound Objects")]
    [SerializeField]
    private GameObject soundBGM, soundClickButton, soundScored, soundBite, soundGameOver;

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
}