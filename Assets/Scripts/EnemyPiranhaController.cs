using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;

public class EnemyPiranhaController : MonoBehaviour
{
    public GameObject shipPlayer;
    public float moveSpeed = 0.1f;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Destroy(this.gameObject);
        }
        else
        {
            #region "Trying to rotate"

            //float radZ = Vector3.Angle(transform.position, shipPlayer.transform.position);

            //if (transform.position.x >= 0)
            //{
            //    transform.eulerAngles = new Vector3(0f, 0f, (-1 * radZ) + 75);
            //}
            //else
            //{
            //    transform.eulerAngles = new Vector3(0f, 0f, (radZ) + 260);
            //}

            #endregion "Trying to rotate"

            FlipMe();
            transform.position += (shipPlayer.transform.position - transform.position) * moveSpeed * Time.deltaTime;
        }
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