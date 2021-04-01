using DragonBones;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//1 ~ 5 -> 2 ~ 5
//5 ~ 10 -> 1 ~ 3
//10 ~ -> 0.5 ~ 1.5
public class SpawnController : MonoBehaviour
{
    [SerializeField]
    private GameObject shipPlayer;

    private GameController gameController;

    private int count = 0;

    private void Awake()
    {
        this.gameController = gameObject.GetComponent<GameController>();
    }

    private void Start()
    {
    }

    public void StartSpawn()
    {
        StartCoroutine("SpawnPiranha");
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private IEnumerator SpawnPiranha()
    {
        float timeToSpawn = 3f;

        if (gameController.level < 6)
        {
            timeToSpawn = Random.Range(2, 5);
        }
        else if (gameController.level < 11)
        {
            timeToSpawn = Random.Range(1, 3);
        }
        else
        {
            timeToSpawn = Random.Range(0.5f, 1.5f);
        }

        yield return new WaitForSeconds(timeToSpawn);

        int scaler = 1;
        for (int i = 0; i < 2; i++)
        {
            MakePiranha(new PiranhaPosition(Random.Range(12, 15) * scaler, Random.Range(-2f, -10f), 0f));
            scaler = -1;
        }

        if (gameController.isGameStarted)
        {
            StartCoroutine("SpawnPiranha");
        }
    }

    private void MakePiranha(PiranhaPosition piranhaPosition)
    {
    }
}

internal class PiranhaPosition
{
    public Vector3 startPosition;

    public PiranhaPosition(float x, float y, float z)
    {
        this.startPosition = new Vector3(x, y, z);
    }

    public float getScaleFactor()
    {
        if (isFromRight())
        {
            return 1;
        }
        return -1;
    }

    public bool isFromRight()
    {
        if (startPosition.x >= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}