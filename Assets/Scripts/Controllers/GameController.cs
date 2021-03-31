using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public bool isGameStarted;
    public bool isBGMActive;

    private GameObject soundControllerBGM;

    [SerializeField]
    private Button btnSoundController;

    [SerializeField]
    private List<Sprite> listSprite;

    // Start is called before the first frame update
    private void Start()
    {
        soundControllerBGM = GameObject.Find("SoundControllerBGM");
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void changeActiveBGM()
    {
        if (isBGMActive)
        {
            soundControllerBGM.SetActive(false);
            btnSoundController.GetComponent<Image>().sprite = listSprite[1];
        }
        else
        {
            soundControllerBGM.SetActive(true);
            btnSoundController.GetComponent<Image>().sprite = listSprite[0];
        }
        isBGMActive = !isBGMActive;
    }
}