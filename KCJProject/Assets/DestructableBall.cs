using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableBall : Destructable
{
    // Start is called before the first frame update
    void Start()
    {
        base.name = "Place Holder for destructable item";
    }

    public override void DoHit() { //overrides the inherited method from the class Destructable
        Debug.Log("You Destroyed " + this.name);
        Destroy(gameObject);
    }
}
