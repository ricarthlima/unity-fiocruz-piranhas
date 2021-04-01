using DragonBones;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//1 ~ 5 -> 2 ~ 5
//5 ~ 10 -> 1 ~ 3
//10 ~ -> 0.5 ~ 1.5
public class SpawnController : MonoBehaviour
{
    [Header("Piranha Prefab")]
    [SerializeField]
    public GameObject piranhaPrefab;

    [Header("Player Ship to Follow")]
    [SerializeField]
    private GameObject shipPlayer;

    private GameController gameController;

    private void Awake()
    {
        this.gameController = gameObject.GetComponent<GameController>();
    }

    private void Start()
    {
    }

    public void StartSpawn()
    {
        SpawnPiranha();
    }

    public void StopSpawn()
    {
        CancelInvoke("SpawnPiranha");
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private float getInterval()
    {
        float timeToSpawn;

        if (gameController.level < 6)
        {
            timeToSpawn = Random.Range(2, 5);
        }
        else if (gameController.level < 11)
        {
            timeToSpawn = Random.Range(1f, 3);
        }
        else
        {
            timeToSpawn = Random.Range(0.3f, 1.5f);
        }

        return timeToSpawn;
    }

    private void SpawnPiranha()
    {
        int scaler = 1;
        for (int i = 0; i < 2; i++)
        {
            GameObject instance = Instantiate(piranhaPrefab);
            instance.transform.position = new Vector3(Random.Range(10.5f, 13) * scaler, Random.Range(-2f, -10f), 0f);
            instance.transform.SetParent(transform);
            scaler = -1;
        }

        if (this.gameController.isGameStarted)
        {
            Invoke("SpawnPiranha", getInterval());
        }
    }
}