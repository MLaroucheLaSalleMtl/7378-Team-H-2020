 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SphereCollider))]
public class BoatInteract : BrokenInteractable
{
    private static TrackerHandler tracker;   
    //[SerializeField] private GameObject exitPanel;
    [SerializeField] private GameObject fixedBoat, exit;
    [SerializeField] private GameObject wrench;
    [Tooltip("Here we attach the text panel telling the player to look for paddles after he fixes the boat")] [SerializeField] private GameObject textPanelFixed;
    
    private bool alreadyInteracted = false;
    private bool isFixed = false;
    [SerializeField] private bool isFixing = false;
    private bool alreadyInsideCollider = false;

    [SerializeField] private AudioSource aSource;

    private AsyncOperation async; // added here because of build bug by julien

    // *** ADDED FOR GAMEPAD SUPPORT BY JULIEN 
    private bool isPressed;

    public void OnBoatFix(InputAction.CallbackContext context)
    {
        if (context.performed)
            isPressed = true;
        else
            isPressed = false;
            
    }
    // *** END OF MODIFICATION

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        tracker = TrackerHandler.instance;
        fixedBoat.SetActive(false);
        gameObject.GetComponent<SphereCollider>().isTrigger = true;
        //exitPanel.gameObject.SetActive(false);
        wrench.gameObject.SetActive(false);
        code = GameManager.instance;
    }

    // Update is called once per frame
    new void Update()
    {
        /* THIS MUST BE CHANGED TO SUPPORT INPUT SYSTEM */
        
        if (isInteractable) {
            if (isPressed == true && pct <= 100) { // LINE MODIFIED BY JULIEN FOR GAMEPAD SUPPORT
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
        if (other.gameObject.tag != "Player") return;
        if (alreadyInteracted == false)
        {
            wrench.gameObject.SetActive(true);
            alreadyInteracted = true;
        }
                
        Wrench w;
        //verifies if player is holding the wrench
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
            textMesh.text = "Hold E / (X) to fix";
        }

        if (!alreadyInsideCollider && alreadyInteracted && isFixed && code.HasTwoPaddles())
        {
            //code.isPopUp = true;
            Debug.Log("You have two paddles and are near the boat");
            //exitPanel.gameObject.SetActive(true);
            //Cursor.lockState = CursorLockMode.None;
            //Cursor.visible = true;
            //code.PauseGame(true);
            alreadyInsideCollider = true;
            code.FinishLevel();

            if (async == null)
            {
                Scene currentScene = SceneManager.GetActiveScene();
                async = SceneManager.LoadSceneAsync(currentScene.buildIndex + 1);
                async.allowSceneActivation = true;
            }
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
        fixedBoat.SetActive(true);
    }

    public override void DoRelease() {
        this.isFixing = false;
    }
}