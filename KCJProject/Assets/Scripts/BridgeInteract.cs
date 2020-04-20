using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


// COMMENT BY JULIEN : THIS SCRIPT WAS USED FOR THIS PUZZLE BUT IS A COPY PASTE OF OUR PUZZLE IN LEVEL 1??? I MODIFIED IT FROM WHAT WAS 
// ORIGINALLY COPIED HERE TO SOMETHING THAT WORKS.

[RequireComponent(typeof(SphereCollider))]

public class BridgeInteract : BrokenInteractable
{   
        private static TrackerHandler tracker;     
        // [SerializeField] private GameObject fixedBridge; REMOVED BY JULIEN
        [SerializeField] private GameObject rope;
        [Tooltip("Here we attach the text panel telling the player to look for paddles after he fixes the boat")] [SerializeField] private GameObject textPanelFixed;
        [SerializeField] private GameObject blockingCollider; // ADDED BY JULIEN

        private bool alreadyInteracted = false;
        private bool isFixed = false;
        [SerializeField] private bool isFixing = false;
        private bool alreadyInsideCollider = false;

        // *** ADDED FOR GAMEPAD SUPPORT BY JULIEN 
        private bool isPressed;

        public void OnBridgeFix(InputAction.CallbackContext context)
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
            //fixedBridge.SetActive(false);  REMOVED BY JULIEN
            gameObject.GetComponent<SphereCollider>().isTrigger = true;
            //exitPanel.gameObject.SetActive(false);
            //rope.gameObject.SetActive(false); REMOVED BY JULIEN
            code = GameManager.instance;
        }

        // Update is called once per frame
        new void Update()
        {
            /* THIS MUST BE CHANGED TO SUPPORT INPUT SYSTEM */

            if (isInteractable)
            {
                if (isPressed == true && pct <= 100)
                { // LINE MODIFIED BY JULIEN FOR GAMEPAD SUPPORT
                    pct = Mathf.Lerp(pct, 125f, Time.deltaTime);
                    this.textMesh.text = pct.ToString("F1") + "%";
                    if (pct >= 100)
                    {
                        //CAll the function because rope is fixed!
                        FixBridge();
                    }
                }
            }
        }

    protected new void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player") return;
        if (alreadyInteracted == false)
        {
            rope.gameObject.SetActive(true);
            alreadyInteracted = true;
        }

        Rope r;
        //verifies if player is holding the rope

        try
        {
            r = code.Holding.GetComponent<Rope>();
        }
        catch
        {
            return;
        }

        if (code.IsHolding && r && !isFixed)
        {
            textMesh.enabled = true;
            isInteractable = true;
            textMesh.text = "Hold E / (X) to fix";
        }
    }
        // ORIGINAL CODE CREATED BY KOMAL BELOW NOT NECESSARY FOR THIS OBJECTIVE

        //if (!alreadyInsideCollider && alreadyInteracted && isFixed && code.HasRope())
        //    {

        //        Debug.Log("You have rope and near the bridge");
        //        //exitPanel.gameObject.SetActive(true);
        //        code.ActivateController(false);
        //        Cursor.lockState = CursorLockMode.None;
        //        Cursor.visible = true;
        //        code.PauseGame(true);
        //        alreadyInsideCollider = true;
        //        //code.FinishLevel();
        //    }
        //}

        //protected new void OnTriggerExit(Collider other)
        //{
        //    alreadyInsideCollider = false;
        //}

        public override void DoInteraction()
        {
            base.DoInteraction();
            this.isFixing = true;
        }

        void FixBridge()
        {
            this.textMesh.text = "";
            isFixed = true;
            blockingCollider.SetActive(false); // ADDED BY JULIEN
            //fixedBridge.SetActive(true); REMOVED BY JULIEN
        }

        public override void DoRelease()
        {
            this.isFixing = false;
        }
    }
