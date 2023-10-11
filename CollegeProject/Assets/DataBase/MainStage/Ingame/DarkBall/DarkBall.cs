using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkBall : MonoBehaviour
{
    [SerializeField]
    Animator animator;

    IEnumerator enumerator;
    Knight player;
    bool once = false;

    // Start is called before the first frame update
    void Start()
    {
        var obj = FindObjectsOfType<Knight>();
        if (obj != null)
            player = obj[0];

        enumerator = Act();
        animator.Play("Spawn");
        StartCoroutine(enumerator);
    }

    // Update is called once per frame
    void Update()
    {
        if((int)player.transform.position.x == (int)transform.position.x && (int)player.transform.position.z == (int)transform.position.z && !once)
        {
            once = true;
            player.Damage(0.75f, 0.5f);
            StopCoroutine(enumerator);
            enumerator = Death();
            StartCoroutine(enumerator);
        }
    }

    IEnumerator Act()
    {
        yield return new WaitForSeconds(0.6f);
        animator.Play("Idle");
        yield return new WaitForSeconds(5f);
        StopCoroutine(enumerator);
        enumerator = Death();
        StartCoroutine(enumerator);
    }

    IEnumerator Death()
    {
        animator.Play("Death");
        yield return new WaitForSeconds(0.8f);
        Destroy(gameObject);
    }
}
