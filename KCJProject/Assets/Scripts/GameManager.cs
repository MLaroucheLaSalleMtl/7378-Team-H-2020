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
    [Tooltip("FPS controller should be attached here")] [SerializeField] private GameObject player;
    [Tooltip("Default healthbar Slider should be attached here")] [SerializeField] private Slider healthbar;
    [Tooltip("Here we enter the amount of health we would like the player to have - Debugging only")][Range(0.0f,100.0f)] [SerializeField] private float health; // This will eventually be a non serialized field, but for development purposes we will keep it here.

    [Tooltip("For Debugging only")] [SerializeField]private bool isHolding = false;
    [Tooltip("For Debugging only")] [SerializeField]private GameObject holding;

    
    [Tooltip("Panel for UI should be attached here")] [SerializeField] private GameObject UI;
    [Tooltip("Panel Inventory should be attached here")] [SerializeField] private GameObject inventoryPanel;

    [Tooltip("Reference for Text Mesh Pro for UI - Add or Drop item")] [SerializeField] private TextMeshProUGUI txtUI;
    private int inventorySize;
    [SerializeField] private int selectedItem;
    public bool pause = false, isInventory = false;

    [Header("Animation Properties")] [SerializeField] private Animator damageAnim;

    [Header("Spawn Points")] [SerializeField] private GameObject levelOneSpawn;

    [Header("Death Panel Prefab")] [SerializeField] private GameObject deathPanel;

    [Header("Ambient Sound")] [Tooltip("Ambient sound SPECIFIC to current level")] [SerializeField] private AudioSource ambientNoise;

    public GameObject Player { get => player; set => player = value; }
    public GameObject Holding { get => holding; set => holding = value; }
    public bool IsHolding { get => isHolding; set => isHolding = value; }
    public int SelectedItem { get => selectedItem; set => selectedItem = value; }
    public static InventoryHandler Ivn { get => ivn; set => ivn = value; }
    public int InventorySize { get => inventorySize; set => inventorySize = value; }

    void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start() {
        Ivn = InventoryHandler.instance;
        InventorySize = Ivn.InventorySize;
        txtUI.text = "";
        healthbar.maxValue = health; // Here we are initializing the healthbar with the value chosen

        deathPanel.SetActive(false);

        Spawn(levelOneSpawn.transform.position);
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void Spawn(Vector3 location)
    {
        player.transform.position = location;
    }

    // Handles pause game (Use this function to pause physics, time and raycast)
    public void PauseGame(bool pause) {
        if (isInventory) return;
        DeactivateController(!pause);
        PauseTime(pause);
        this.pause = pause;
    }
    //activates and deactivates player controls
    public void DeactivateController(bool flag) {
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
        if (pause) return;
        if (context.performed) {
            if (!isInventory) { //If tab is pressed and the inventory is not showing -> show inventory
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                inventoryPanel.SetActive(true);
                UI.SetActive(false);
                DeactivateController(false);
                                
                Ivn.DescMenu(false);
            } else { //If tab is pressed and the inventory is showing -> close inventory
                inventoryPanel.SetActive(false);
                UI.SetActive(true);
                DeactivateController(true);
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

    public void CheckIsPlayerDead()
    {
        if (health <= 0)
        {
            ambientNoise.volume = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            PauseGame(true);
            deathPanel.SetActive(true);
            health = 0;
            damageAnim.SetTrigger("StopDamage");
            //Broadcasts the player died. For handling of death use the function 'Dead' in any script that needs to handle death
            BroadcastMessage("Dead", SendMessageOptions.DontRequireReceiver);
        }
    }

    public void FlowerDamage(float flowerDamageOverTime)
    {
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
    public void GiveHealth(int healPoints) {
        health += healPoints;
        Debug.Log(health);
        if (health >= 100) health = 100;
        healthbar.value = health; //updates slider
    }
    //Handles player death
    private void Dead() {
        Debug.Log("You are dead");
    }
}
