using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;
using System;

public class EnemyPiranhaController : MonoBehaviour
{
    private GameController gameController;

    private GameObject shipPlayer;
    public float moveSpeed = 0.1f;
    public float clickTolerance = 1f;

    private Vector3 leftDeathPoint = new Vector3(-10f, -2f, 0f);
    private Vector3 rightDeathPoint = new Vector3(10f, -2f, 0f);

    private bool isDying = false;

    // Start is called before the first frame update
    private void Start()
    {
        this.shipPlayer = GameObject.Find("PlayerShip");
        this.gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (isDying)
        {
            if (transform.position.x < 0)
            {
                transform.position += (leftDeathPoint - transform.position) * 4f * Time.deltaTime;
            }
            else
            {
                transform.position += (rightDeathPoint - transform.position) * 4f * Time.deltaTime;
            }
        }
        else
        {
            FlipMe();
            transform.position += (shipPlayer.transform.position - transform.position) * moveSpeed * Time.deltaTime;
        }
    }

    private void OnMouseDown()
    {
        Destroy(gameObject, 3f);
        transform.localScale = new Vector3(transform.localScale.x * -1, 0.25f, 0.25f);
        isDying = true;
        this.gameController.MakeAPoint(10);
    }

    private bool DidTouchThat(Vector3 touch, Vector3 position)
    {
        if (Math.Abs(touch.x - touch.x) <= clickTolerance)
        {
            if (Math.Abs(touch.y - touch.y) <= clickTolerance)
            {
                return true;
            }
        }
        return false;
    }

    private void FlipMe()
    {
        if (transform.position.x < 0)
        {
            transform.localScale = new Vector3(-0.25f, 0.25f, 0.25f);
        }
        else
        {
            transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        }
    }

    private void FixedUpdate()
    {
    }
}