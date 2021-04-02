using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  Autor do Script:            Ricarth Lima
 *  Data Script:                01/04/2021
 *  Versão do Script:           1.0
 *  Finalidade do Script:       Faz com que o GameObject que receba esse script se
 *                              auto destrua depois de um tempo.
 */

public class DestroyMe : MonoBehaviour
{
    public float timeToDie = 1.2f;

    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine("Die");
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private IEnumerator Die()
    {
        yield return new WaitForSeconds(timeToDie);
        Destroy(gameObject);
    }
}