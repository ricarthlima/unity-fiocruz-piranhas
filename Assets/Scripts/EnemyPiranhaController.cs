using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;
using System;

public class EnemyPiranhaController : MonoBehaviour
{
    private GameObject shipPlayer;
    public float moveSpeed = 0.1f;
    public float clickTolerance = 1f;

    // Start is called before the first frame update
    private void Start()
    {
        this.shipPlayer = GameObject.Find("PlayerShip");
    }

    // Update is called once per frame
    private void Update()
    {
        FlipMe();
        transform.position += (shipPlayer.transform.position - transform.position) * moveSpeed * Time.deltaTime;
    }

    private void LateUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            print("Touch: " + touchPosition.ToString());
            print("Position: " + transform.position.ToString());

            if (DidTouchThat(touchPosition, transform.position))
            {
            }
        }
    }

    private void OnMouseDown()
    {
        Destroy(gameObject);
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