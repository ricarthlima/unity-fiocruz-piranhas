using System.Collections;
using UnityEngine;

/*
 *  Autor do Script:            Ricarth Lima
 *  Data Script:                01/04/2021
 *  Vers?o do Script:           1.0
 *  Finalidade do Script:       Controla o comportamento das piranhas comuns.
 */

public class EnemyPiranhaController : MonoBehaviour
{
    private GameController gameController;

    private GameObject shipPlayer;
    public float moveSpeed = 0.1f;

    private Vector3 leftDeathPoint = new Vector3(-20f, -2f, 0f);
    private Vector3 rightDeathPoint = new Vector3(20f, -2f, 0f);

    public bool isDying = false;

    // Start is called before the first frame update
    private void Start()
    {
        this.shipPlayer = GameObject.Find("PlayerShip");
        this.gameController = GameObject.Find("GameController").GetComponent<GameController>();
        moveSpeed += (gameController.level / 20f);
    }

    // Update is called once per frame
    private void Update()
    {
        if (isDying)
        {
            if (transform.position.x < 0)
            {
                transform.position += (leftDeathPoint - transform.position) * 2f * Time.deltaTime;
            }
            else
            {
                transform.position += (rightDeathPoint - transform.position) * 2f * Time.deltaTime;
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
        Destroy(gameObject, 1.5f);
        transform.localScale = new Vector3(transform.localScale.x * -1, 0.25f, 0.25f);
        isDying = true;
        this.gameController.MakeAPoint(9);
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

    public void HasCollided()
    {
        GetComponent<Animator>().SetBool("biting", true);
        Destroy(GetComponent<BoxCollider2D>());
        StartCoroutine("Flee");
    }

    private IEnumerator Flee()
    {
        yield return new WaitForSeconds(1.5f);
        GetComponent<Animator>().SetBool("biting", false);
        isDying = true;
    }
}