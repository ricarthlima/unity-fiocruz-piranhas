using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpeedsY
{
    public const float baseSpeed = 0.6f;
    public const float minimunSpeed = 0.075f;
}

public class ShipController : MonoBehaviour
{
    private GameController gameContoller;

    [SerializeField]
    private float _speedInY, _speedInX;

    private bool _isGoingUp;
    private bool _isGoingRight;

    // Start is called before the first frame update
    private void Start()
    {
        GameObject gameControllerObject = GameObject.Find("GameController");
        this.gameContoller = gameControllerObject.GetComponent<GameController>();

        _speedInY = SpeedsY.baseSpeed;
        _speedInX = 0.4f;
    }

    // Update is called once per frame
    private void Update()
    {
        if (this.gameContoller.isGameStarted)
        {
            riverMovementX();
        }
        else
        {
            transform.position = new Vector3(0f, transform.position.y, 0f);
        }

        riverMovementY();
    }

    private void riverMovementX()
    {
        if (_isGoingRight && transform.position.x < 4.5f)
        {
            transform.position += transform.right * _speedInX * Time.deltaTime;
            if (transform.position.x >= 4.5f)
            {
                _isGoingRight = false;
            }
        }

        if (!_isGoingRight && transform.position.x > -4.5f)
        {
            transform.position += -transform.right * _speedInX * Time.deltaTime;
            if (transform.position.x <= -4.5f)
            {
                _isGoingRight = true;
            }
        }
    }

    private void riverMovementY()
    {
        if (!_isGoingUp && transform.position.y > 0)
        {
            if (transform.position.y <= 0.15)
            {
                _speedInY -= 0.9f * Time.deltaTime;
                _speedInY = Mathf.Max(SpeedsY.minimunSpeed, _speedInY);
            }
            transform.position += -transform.up * _speedInY * Time.deltaTime;
            if (transform.position.y <= 0)
            {
                _isGoingUp = true;
                _speedInY = SpeedsY.baseSpeed;
            }
        }

        if (_isGoingUp && transform.position.y < 1)
        {
            if (transform.position.y >= 0.85)
            {
                _speedInY -= 1f * Time.deltaTime;
                _speedInY = Mathf.Max(SpeedsY.minimunSpeed, _speedInY);
            }
            transform.position += transform.up * _speedInY * Time.deltaTime;
            if (transform.position.y >= 1)
            {
                _isGoingUp = false;
                _speedInY = SpeedsY.baseSpeed;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyPiranhaController>().HasCollided();
            this.gameContoller.TakeADamage();
        }
    }
}