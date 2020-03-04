using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grabbable : InteractableHandler {
    protected static GameManager code;
    protected Camera cam;
    protected bool isChild = false;
    protected Rigidbody body;
    [SerializeField] protected Sprite icon;

    [Tooltip("Force added to dropped item - 1 drops item straight to the floor")][SerializeField]protected float dropForce = 1f;
    [Tooltip("Object name - It will be displayed on the inventory")][SerializeField] protected string name;

    public string Name { get => name; set => name = value; }
    public Sprite Icon { get => icon; set => icon = value; }

    [Tooltip("Check if standing or uncheck if the object is layingdown")][SerializeField] private bool isStanding = false;

    // Start is called before the first frame update
    new void Start() {
        base.Start();
        code = GameManager.instance;
        cam = code.Player.GetComponentsInChildren<Camera>()[1];
        body = GetComponent<Rigidbody>();
        base.setMesh();
    }

    public override void DoInteraction() {
        base.DoInteraction();
        code = GameManager.instance;
        if (code.inventory.Count >= code.InventorySize) return;
        //if there is something in hand, put item in inventory and not hands
        if (!code.IsHolding) {
            code.Holding = this.gameObject;
            code.IsHolding = true;
        } else {
            this.gameObject.SetActive(false);
        }
        code.AddInventory(this.gameObject);
    }
    protected void ChangeLayer(int layer) {
        Transform[] children = GetComponentsInChildren<Transform>();
        this.gameObject.layer = layer;
        foreach (Transform child in children) {
            child.gameObject.layer = layer;
        }
    }
    public void OnDrop() {
        if (code.Holding == this.gameObject) {
            code.IsHolding = false;
            code.Holding = null;
        }
        gameObject.transform.parent = null;
        isChild = false;
        body.isKinematic = false;
        body.detectCollisions = true;
        this.textMesh.enabled = true;
        this.ChangeLayer(8);
        body.AddForce(transform.forward * dropForce);
        this.gameObject.SetActive(true);
        code.DropInventory(this.gameObject);
    }
    protected virtual void ShowInHands() {
        body = GetComponent<Rigidbody>();
        cam = code.Player.GetComponentsInChildren<Camera>()[1];
        //Deactivates the rigid body
        body.isKinematic = true;
        body.detectCollisions = false;
        //Makes the object a child of the player
        gameObject.transform.SetParent(cam.transform);
        gameObject.transform.position = cam.transform.position;

        isChild = true;
        this.textMesh.enabled = false;
        this.ChangeLayer(9);
        
    }
}
