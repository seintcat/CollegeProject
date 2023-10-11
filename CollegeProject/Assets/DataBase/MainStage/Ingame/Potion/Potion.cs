using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : Item
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnEvent(Actor actor)
    {

    }

    public override void UnEquipEvent(Actor actor)
    {

    }

    public override void UseEvent(Actor actor)
    {
        Knight knight = (Knight)actor;
        if (knight != null)
            knight.Heal(0.5f);
    }
}
