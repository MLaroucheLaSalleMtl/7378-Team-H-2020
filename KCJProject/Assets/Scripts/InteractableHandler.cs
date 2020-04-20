using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Animations;


[RequireComponent(typeof(SphereCollider))] //makes sure there is always a sphere collider attached to the interactable object
public class InteractableHandler : MonoBehaviour
{
    [SerializeField] protected TextMeshPro textMesh;
    [SerializeField] protected float radius = 3f;
    [SerializeField] protected string functionName;
    private BoxAnimation boxAnim;
    protected SphereCollider range; //sphere collider to test if the player is inside the range to be interactable
    protected bool isInteractable = false;

    // Start is called before the first frame update
    protected void Start() {
        gameObject.GetComponent<SphereCollider>().isTrigger = true; //makes sure the sphere collider is always a trigger
        if (string.IsNullOrWhiteSpace(functionName)) functionName = gameObject.name;
        setMesh();
        range = GetComponent<SphereCollider>();
        range.radius = radius;
        boxAnim = GetComponent<BoxAnimation>();
    }

    // Update is called once per frame
    protected void Update()
    {

    }

    public virtual void DoInteraction() {
        if (!isInteractable) return;
    }

    protected void setMesh() {
        textMesh.SetText(functionName + " \n E / (X)");
        textMesh.enabled = false;
    }
    protected virtual void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            textMesh.enabled = true;
            isInteractable = true;
        }
    }
    protected virtual void OnTriggerExit(Collider other) {
        textMesh.enabled = false;
        isInteractable = false;
    }
}
