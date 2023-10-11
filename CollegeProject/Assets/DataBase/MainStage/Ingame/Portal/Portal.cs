using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Portal : MonoBehaviour
{
    public Vector2 teleport;
    public UnityEvent events;

    private void Awake()
    {
        StartCoroutine(PortalMove());
    }
    
    IEnumerator PortalMove()
    {
        yield return null;
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            var player = FindObjectOfType<Knight>();
            if (player != null && player.transform.position.x == transform.position.x && player.transform.position.z == transform.position.z)
            {
                player.teleport = teleport;
                if (!(events.GetPersistentEventCount() < 1))
                    events.Invoke();
            }
        }
    }
}
