using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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