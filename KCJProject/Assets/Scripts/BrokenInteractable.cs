using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenInteractable : InteractableHandler {
    [Tooltip("Inspector for debugging purposes only - Do not change this value")]
    [SerializeField] protected float pct = 0;
    protected static GameManager code;
    // Start is called before the first frame update
    new void Start() {
        base.Start();
        code = GameManager.instance;
    }

    // Update is called once per frame
    new void Update() {
        
    }

    public override void DoInteraction() {
        base.DoInteraction();
    }


    private new void OnTriggerEnter(Collider other) {
        
    }
    public virtual void DoRelease() {

    }
}