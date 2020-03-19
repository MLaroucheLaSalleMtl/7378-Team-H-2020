using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxAnimation : InteractableHandler
{
    protected static GameManager code;
    [SerializeField] private Animator objectAnimation;
    [Tooltip("How many seconds before destroing the object after interaction")]
    [SerializeField] private float destroyAfter;
    private GameObject box;
    new void Start() {
        base.Start();
        code = GameManager.instance;
    }
    public override void DoInteraction()
    {
        if (!isInteractable) return;
        objectAnimation.SetBool("Opened", true);
        Invoke("DestroyAfter", destroyAfter); //Calls the method to destroy the game object after sometime
        isInteractable = false; //Resets the interaction
        base.textMesh.enabled = false;
    }

    private void DestroyAfter() {
        Destroy(gameObject);
    }
}