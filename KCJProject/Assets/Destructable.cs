using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Destructable : MonoBehaviour {

    [SerializeField] protected string name;
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public virtual void DoHit() { //To be implemented in any children class
        //when hit by raycast, call this function
    }
}
