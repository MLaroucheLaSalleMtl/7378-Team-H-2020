using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grabbable : InteractableHandler {
    private static GameManager code;
    private Camera cam;
    private bool isChild = false;
    private Rigidbody body;
    [SerializeField]private Sprite icon;

    [Tooltip("Force added to dropped item - 1 drops item straight to the floor")][SerializeField]private float dropForce = 1f;
    [Tooltip("Object name - It will be displayed on the inventory")][SerializeField] private string name;

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
        if (code.inventory.Count >= code.InventorySize) return;
        //if there is something in hand, put item in inventory and not hands
        if (!code.IsHolding) {
            ShowInHands();
            code.Holding = this.gameObject;
            code.IsHolding = true;
        } else { 
            ShowInHands();
            this.gameObject.SetActive(false);
        }
        code.AddInventory(this.gameObject);
    }
    private void ChangeLayer(int layer) {
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
    public void ShowInHands() {
        //Deactivates the rigid body
        body.isKinematic = true;
        body.detectCollisions = false;
        //Makes the object a child of the player
        gameObject.transform.SetParent(cam.transform);
        gameObject.transform.position = cam.transform.position;
        //Sets the object position relatable to the camera
        gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x + 0.24f, gameObject.transform.localPosition.y - 0.12f, gameObject.transform.localPosition.z + 0.47f);
        gameObject.transform.localRotation = cam.transform.localRotation;
        if(!isStanding)gameObject.transform.localEulerAngles = new Vector3(0, gameObject.transform.localEulerAngles.y + 90f, gameObject.transform.localEulerAngles.z + 90);
        else gameObject.transform.localEulerAngles = new Vector3(0, gameObject.transform.localEulerAngles.y, gameObject.transform.localEulerAngles.z);

        isChild = true;
        this.textMesh.enabled = false;
        this.ChangeLayer(9);
    }
}
