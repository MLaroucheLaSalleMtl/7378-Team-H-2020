using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BoxAnimation : InteractableHandler
{
    protected static GameManager code;
    [SerializeField] private Animator objectAnimation;
    [Tooltip("How many seconds before destroing the object after interaction")]
    [SerializeField] private float destroyAfter;
    private GameObject box;

    [SerializeField] private AudioSource aSource;

    public void OnOpen(InputAction.CallbackContext context)
    {
        if (context.performed)
            DoInteraction();
    }

    new void Start() {
        base.Start();
        code = GameManager.instance;
    }

    public override void DoInteraction()
    {
        if (!isInteractable) return;
        objectAnimation.SetBool("Opened", true);
        aSource.Play();
        Invoke("Heal", 1);
        Invoke("DestroyAfter", destroyAfter);
        isInteractable = false; //Resets the interaction
        base.textMesh.enabled = false;
    }

    private void DestroyAfter() { // changed by julien so the object does not dissapear after using it
        base.textMesh.SetText("Empty Medical Box");
        base.textMesh.enabled = true;
    }

    private void Heal()
    {
        code.GiveHealth(15);
    }
}