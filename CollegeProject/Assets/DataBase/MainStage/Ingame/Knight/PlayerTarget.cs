using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTarget : MonoBehaviour
{
    public Transform target;

    public Animator animator;

    public Knight knight;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        int x, z;
        if (Input.GetKey(KeyCode.W))
            z = 1;
        else if (Input.GetKey(KeyCode.S))
            z = -1;
        else
            z = 0;

        if (Input.GetKey(KeyCode.A))
            x = -1;
        else if (Input.GetKey(KeyCode.D))
            x = 1;
        else
            x = 0;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (InGameUIManager.paused)
                InGameUIManager.Resume();
            else
                InGameUIManager.Pause();
        }

        if (knight.canMove && !knight.killed)
        {
            if (x == 0 && z == 0)
                animator.Play("Idle");
            else
                animator.Play("Run");
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (Item item in knight.itemList)
                if (item.itemCode == 0)
                {
                    knight.UseItem(item);
                    break;
                }
        }
        
        transform.position = new Vector3(target.position.x + x, transform.position.y, target.position.z + z);
    }
}
