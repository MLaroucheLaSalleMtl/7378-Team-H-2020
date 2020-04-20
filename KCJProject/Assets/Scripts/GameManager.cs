using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;


public class GameManager : MonoBehaviour {
    public static GameManager instance = null;
    private static InventoryHandler ivn;
    private static CheckpointHandler checkPointHandlerCode;
    [Header("Level")]
    [Tooltip("Level for this Game Manager")] [SerializeField]private int level;
    [Tooltip("FPS controller should be attached here")] [SerializeField] private GameObject player;
    [Tooltip("Default healthbar Slider should be attached here")] [SerializeField] private Slider healthbar;
    [Tooltip("Here we enter the amount of health we would like the player to have - Debugging only")][Range(0.0f,100.0f)] [SerializeField] private float health; // This will eventually be a non serialized field, but for development purposes we will keep it here.
    //[Tooltip("Default stamina slider should be attached here")] [SerializeField] private Slider staminaBar;
    //[Tooltip("Here we enter the amount of stamina we would like the player to have - Debugging only")][Range(0.0f, 100.0f)] [SerializeField] private float stamina;


    [Tooltip("For Debugging only")] [SerializeField]private bool isHolding = false;
    [Tooltip("For Debugging only")] [SerializeField]private GameObject holding;

    
    [Tooltip("Panel for UI should be attached here")] [SerializeField] private GameObject UI;
    [Tooltip("Panel Inventory should be attached here")] [SerializeField] private GameObject inventoryPanel;

    [Tooltip("Reference for Text Mesh Pro for UI - Add or Drop item")] [SerializeField] private TextMeshProUGUI txtUI;
    private int inventorySize;
    [SerializeField] private int selectedItem;
    [HideInInspector] public bool pause = false, isInventory = false, isTracker = false, isPopUp = false, isDeath = false; // isPopUP and isDeath added by julien

    [Header("Animation Properties")] [SerializeField] private Animator damageAnim;
    [SerializeField] public GameObject damagePanel;

    [Header("Death Panel Prefab")] [SerializeField] public GameObject deathPanel;

    [Header("Ambient Sound")] [Tooltip("Ambient sound SPECIFIC to current level")] [SerializeField] private AudioSource ambientNoise;

    [Header("Death Panel Default Button")] [SerializeField] private Selectable defaultSelected_onDeath;

    [HideInInspector] public bool reset = false;
    /*
    * This public boolean is used in other scripts such as flower damage to know if player is being 
    * reset (respawned) to stop any damage incoming since we are not calling the OnTriggerExit located 
    * inside these scripts when moving the player on respawn
    */

    [HideInInspector] public SetPos setPos;
    [HideInInspector] public SetInventory setInventory;
    [HideInInspector] public SetGameLevel setLevel;

    public GameObject Player { get => player; set => player = value; }
    public GameObject Holding { get => holding; set => holding = value; }
    public float Health { get => health; set => health = value; }
    //public float Stamina { get => stamina; set => stamina = value; }
    public bool IsHolding { get => isHolding; set => isHolding = value; }
    public int SelectedItem { get => selectedItem; set => selectedItem = value; }
    public static InventoryHandler Ivn { get => ivn; set => ivn = value; }
    public int InventorySize { get => inventorySize; set => inventorySize = value; }
    public int Level { get => level; set => level = value; }

    void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }
    }


    // Start is called before the first frame update
    void Start() {
        checkPointHandlerCode = CheckpointHandler.instance;
        Ivn = InventoryHandler.instance;
        //checkPointHandlerCode = CheckpointHandler.instance;
        InventorySize = Ivn.InventorySize;
        txtUI.text = "";

        deathPanel.SetActive(false);

        player.SetActive(false);
        setPos = GetComponent<SetPos>();
        setInventory = GetComponent<SetInventory>();
        setLevel = GetComponent<SetGameLevel>();
        checkPointHandlerCode.Spawn();
        Init();
    }

    //Initialize each level
    private void Init() {
        setLevel.SetLevel();
        setLevel.Write();
        if (Level == 1) {
            healthbar.maxValue = health; // Here we are initializing the healthbar with the value chosen
        } else {
            checkPointHandlerCode.Read();
        }
       
    }
    
    public void FinishLevel() {
        checkPointHandlerCode.lastCheckPointPos = Player.transform.position;
        checkPointHandlerCode.lastCheckPointRot = Player.transform.rotation;
        checkPointHandlerCode.GetCurrentHealth();
        checkPointHandlerCode.Save();
    }

    // Update is called once per frame
    void Update() {

    }


    // Handles pause game (Use this function to pause physics, time and raycast)
    public void PauseGame(bool pause) {
        if (isInventory || isTracker) return;
        ActivateController(!pause);
        PauseTime(pause);
        this.pause = pause;
    }
    //activates and deactivates player controls False -> deactivates True -> activate
    public void ActivateController(bool flag) {
        Player.GetComponent<FirstPersonController>().enabled = flag;
        Player.GetComponent<CharacterController>().enabled = flag;
    }
    //Handles timescale
    public void PauseTime(bool flag) {
        if (flag) Time.timeScale = 0f;
        else Time.timeScale = 1f;
    }
    public void OnDrop(InputAction.CallbackContext context) {
        if (!IsHolding) return;
        if (context.performed)
            Holding.GetComponent<Grabbable>().OnDrop();
    }
    public void OnTab(InputAction.CallbackContext context) {//Enters Inventory
        if (pause || isTracker) return;
        if (context.performed) {
            if (!isInventory) { //If tab is pressed and the inventory is not showing -> show inventory
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                inventoryPanel.SetActive(true);
                Ivn.HighlightIvn();
                UI.SetActive(false);
                ActivateController(false);
                                
                Ivn.DescMenu(false);
            } else { //If tab is pressed and the inventory is showing -> close inventory
                inventoryPanel.SetActive(false);
                UI.SetActive(true);
                ActivateController(true);
                SelectedItem = 0;
            }
            isInventory = !isInventory;
        }
    }
    public void AddInventory(GameObject obj) {
        Ivn.Add(obj);
        ActivateTxt("Added " + obj.GetComponent<Grabbable>().Name);
    }
    public void DropInventory(GameObject obj) {
        Ivn.Drop(obj,SelectedItem);
        ActivateTxt("Dropped " + obj.GetComponent<Grabbable>().Name);
    }

    public void SelectItem(int item) {
        if (item > Ivn.inventory.Count) return;
        Ivn.SelectItem(item);
        SelectedItem = item;
        Ivn.DescMenu(true);        
    }
    public void HoldItem() {
        if (Ivn.inventory.Count <= 0) return;
        Holding = Ivn.inventory[SelectedItem].gameObject;
        Holding.SetActive(true);
        Ivn.HoldItem(Holding);
        isHolding = true;
    }
    public void DropFromMenu() {
        if (Ivn.inventory.Count <= 0) return;
        Debug.Log(SelectedItem);
        Ivn.DropFromMenu(SelectedItem);
    }
    public void ActivateTxt(string s) {
        txtUI.text = s;
        Invoke("DeactivateTxt", 5f);
    }
    public void DeactivateTxt() {
        txtUI.text = "";
    }

    public bool HasTwoPaddles()
    {
        int paddles = 0;

        foreach (GameObject item in Ivn.inventory)
        {
            if (item.gameObject.tag == "Paddle")
            {
                paddles += 1;
            }
        }

        if (paddles >= 2)
        {
            return true;
        } else
        {
            return false;
        }
    }
    public bool HasRope()
    {
        int rope = 0;
        foreach(GameObject item in Ivn.inventory)
        {
            if(item.gameObject.tag == "Rope")
            {
                rope += 1;
            }

        }
        return true;
    }
    public void CheckIsPlayerDead()
    {
        if (health <= 0)
        {            
            //Broadcasts the player died. For handling of death use the function 'Dead' in any script that needs to handle death
            BroadcastMessage("Dead", SendMessageOptions.DontRequireReceiver);
        }
    }

    public void DeactivateDeath()
    {
        ambientNoise.volume = 100;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PauseGame(false);
        deathPanel.SetActive(false);
        reset = true;
    }

    public void FlowerDamage(float flowerDamageOverTime)
    {
        if (health <= 0) return;
        health -= flowerDamageOverTime * Time.deltaTime;
        damageAnim.SetTrigger("Damage");
        healthbar.value = health; //updates slider
        CheckIsPlayerDead();
    }

    public void StopDamageAnim()
    {
        damageAnim.SetTrigger("StopDamage");
    }

    public void LoseHealth(int lostHealth)
    {
        health -= lostHealth;
        healthbar.value = health; //updates slider
        CheckIsPlayerDead();
    }
    public void GiveHealth(float healPoints) {
        health += healPoints;
        Debug.Log(health);
        if (health >= 100) health = 100;
        healthbar.value = health; //updates slider
    }
    //Handles player death
    private void Dead() {
        isDeath = true;
        ambientNoise.volume = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PauseGame(true);
        deathPanel.SetActive(true);
        deathPanel.GetComponentsInChildren<Selectable>()[0].Select(); // try to pre select a button for gamepad support
        health = 0;
        damageAnim.SetTrigger("StopDamage");
    }

    //public void SprintStaminaLoss(float staminaLossOverSprint)
    //{
    //    stamina -= staminaLossOverSprint * Time.deltaTime;
    //    staminaBar.value = stamina; // updates slider
    //}


    //public bool isRunning()
    //{
    //    if (Player.GetComponent<FirstPersonController>().walk == true)
    //    {
    //        return true;
    //    } else
    //    {
    //        return false;
    //    }
    //}
}
