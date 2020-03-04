 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(SphereCollider))]
public class BoatInteract : BrokenInteractable
{
    
    [SerializeField] private GameObject textPanel;
    [SerializeField] private GameObject exitPanel;
    [SerializeField] private GameObject wrench;
    [SerializeField] private AudioSource alertSound;
    [Tooltip("Here we attach the text panel telling the player to look for paddles after he fixes the boat")] [SerializeField] private GameObject textPanelFixed;
    private bool alreadyInteracted = false;
    private bool isFixed = false;
    [SerializeField] private bool isFixing = false;
    private bool alreadyInsideCollider = false;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        gameObject.GetComponent<SphereCollider>().isTrigger = true;
        textPanel.gameObject.SetActive(false);
        exitPanel.gameObject.SetActive(false);
        wrench.gameObject.SetActive(false);
        code = GameManager.instance;
    }

    // Update is called once per frame
    new void Update()
    {
        /* THIS MUST BE CHANGED TO SUPPORT INPUT SYSTEM */
        if (Input.GetKeyDown(KeyCode.Return))
        {
            textPanel.gameObject.SetActive(false);
            textPanelFixed.gameObject.SetActive(false);
        }
        if (isInteractable) {
            if (Input.GetKey("e") && pct <= 100) {
                pct = Mathf.Lerp(pct, 125f, Time.deltaTime);
                this.textMesh.text = pct.ToString("F1") + "%";
                if (pct >= 100) {
                    //CAll the function because boat is fixed!
                    FixBoat();
                }
            }
        }
    }

    protected new void OnTriggerEnter(Collider other)
    {
        if (alreadyInteracted == false)
        {
            textPanel.gameObject.SetActive(true);
            alertSound.Play();
            wrench.gameObject.SetActive(true);
            alreadyInteracted = true;
        }
        if (other.gameObject.tag != "Player") return;
        Wrench w;
        //verifies if playes is holding the wrench
        try
        {
            w = code.Holding.GetComponent<Wrench>();
        }
        catch
        {
            return;
        }
        if (code.IsHolding && w && !isFixed)
        {
            textMesh.enabled = true;
            isInteractable = true;
            textMesh.text = "Hold (E) to fix";
        }

        if (!alreadyInsideCollider && alreadyInteracted && isFixed && code.HasTwoPaddles())
        {
            
            Debug.Log("You have two paddles and are near the boat");
            exitPanel.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            code.PauseGame(true);
            alreadyInsideCollider = true;
        }
    }

    protected new void OnTriggerExit(Collider other)
    {
        alreadyInsideCollider = false;
    }

    public override void DoInteraction() {
        base.DoInteraction();
        this.isFixing = true;
    }
    void FixBoat()
    {
        this.textMesh.text = "";
        isFixed = true;
        textPanelFixed.SetActive(true);
        alertSound.Play();
    }

    public override void DoRelease() {
        this.isFixing = false;
    }
}