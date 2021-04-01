using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [Header("Sound Objects")]
    [SerializeField]
    private GameObject soundBGM, soundClickButton;

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
        soundClickButton.SetActive(true);
        StartCoroutine("DesableButtonSound");
    }

    private IEnumerator DesableButtonSound()
    {
        yield return new WaitForSeconds(0.7f);
        this.soundClickButton.SetActive(false);
    }
}