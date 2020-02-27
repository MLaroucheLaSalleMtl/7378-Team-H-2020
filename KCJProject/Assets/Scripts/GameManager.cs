using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour {
    public static GameManager instance = null;
    [Tooltip("FPS controller should be attached here")] [SerializeField] private GameObject player;
    [Tooltip("Default healthbar Slider should be attached here")] [SerializeField] private Slider healthbar;
    [Tooltip("Here we enter the amount of health we would like the player to have")][Range(0.0f,100.0f)] [SerializeField] private float health; // This will eventually be a non serialized field, but for development purposes we will keep it here.

    [Tooltip("For Debugging only")] [SerializeField]private bool isHolding = false;
    [Tooltip("For Debugging only")] [SerializeField]private GameObject holding;

    [Tooltip("Panel for UI should be attached here")] [SerializeField] private GameObject UI;
    [Tooltip("Panel Inventory should be attached here")] [SerializeField] private GameObject inventoryPanel;

    [Tooltip("Inventory Size")]
    [SerializeField] private int inventorySize;
    [Tooltip("List of Objects on inventory")]
    public List<GameObject> inventory = new List<GameObject>();

    [Header("Inventory - UI")]
    [Tooltip("List of inventory slots - Images")] [SerializeField] private Image[] iconInventory;
    [Tooltip("List of inventory slots - Buttons")] [SerializeField] private Selectable[] btnItens;
    [Tooltip("Default Image for empty slot on inventory")] [SerializeField] private Sprite emptyObj;

    [Tooltip("Text UI that shows the name of the selected object")] [SerializeField] private Text txtName;
    [Tooltip("Image UI that shows the icon of the selected object")] [SerializeField] private Image descImg;
    [Tooltip("Reference of Equip Button should be attached here")] [SerializeField] private Selectable btnEquip;
    [Tooltip("Reference of Drop Button should be attached here")] [SerializeField] private Selectable btnDrop;

    [Tooltip("Reference for Text Mesh Pro for UI - Add or Drop item")] [SerializeField] private TextMeshProUGUI txtUI;

    [SerializeField]private int selectedItem;
    public bool pause = false, isInventory = false;

    public GameObject Player { get => player; set => player = value; }
    public GameObject Holding { get => holding; set => holding = value; }
    public bool IsHolding { get => isHolding; set => isHolding = value; }
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
        foreach (Image img in iconInventory) img.sprite = emptyObj;
        foreach (Selectable btn in btnItens) btn.interactable = false;
        UpdateInventory();
        txtUI.text = "";

        healthbar.maxValue = health; // Here we are initializing the healthbar with the value chosen
    }

    // Update is called once per frame
    void Update() {
        healthbar.value = health; // The slider will get the value of health in the update function so we can manipulate easily simply using the health variable.
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
            if (!isInventory) {
                inventoryPanel.SetActive(true);
                UI.SetActive(false);
                DeactivateController(false);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                descImg.sprite = emptyObj;
                DescMenu(false);
            } else {
                inventoryPanel.SetActive(false);
                UI.SetActive(true);
                DeactivateController(true);
                selectedItem = 0;
            }
            isInventory = !isInventory;
        }
    }
    public void AddInventory(GameObject obj) {
        inventory.Add(obj);
        this.iconInventory[inventory.Count - 1].sprite = obj.GetComponent<Grabbable>().Icon;
        this.btnItens[inventory.Count - 1].interactable = true;
        UpdateInventory();
        ActivateTxt("Added " + obj.GetComponent<Grabbable>().Name);
    }
    public void DropInventory(GameObject obj) {
        inventory.Remove(obj);
        iconInventory[selectedItem].sprite = emptyObj;
        btnItens[selectedItem].interactable = false;        
        UpdateInventory();
        ActivateTxt("Dropped " + obj.GetComponent<Grabbable>().Name);
    }
    public void SelectItem(int item) {
        if (item > inventory.Count) return;
        selectedItem = item;
        descImg.sprite = inventory[selectedItem].GetComponent<Grabbable>().Icon;
        txtName.text = inventory[selectedItem].GetComponent<Grabbable>().Name;
        DescMenu(true);
    }
    public void HoldItem() {
        if (inventory.Count <= 0) return;
        Holding = inventory[selectedItem].gameObject;
        Holding.SetActive(true);
        foreach(GameObject item in inventory) {
            if (item != Holding) item.SetActive(false);
        }
        isHolding = true;
    }
    public void UpdateInventory() {
        if(inventory.Count <= 0) {
            txtName.enabled = false;
            descImg.sprite = emptyObj;
            selectedItem = 0;
        } else {
            txtName.enabled = true;
            int i = 0;
            foreach(GameObject item in inventory) {
                iconInventory[i].sprite = item.GetComponent<Grabbable>().Icon;
                btnItens[i].interactable = true;
                i++;
            }
            for(int z = i; z < inventorySize; z++) {
                iconInventory[z].sprite = emptyObj;
                btnItens[z].interactable = false;
            }
        }
    }
    public void DescMenu(bool flag) {
        txtName.enabled = flag;
        btnDrop.interactable = flag;
        btnEquip.interactable = flag;
    }
    public void DropFromMenu() {
        inventory[selectedItem].GetComponent<Grabbable>().OnDrop();     
    }
    public void ActivateTxt(string s) {
        txtUI.text = s;
        Invoke("DeactivateTxt", 5f);
    }
    public void DeactivateTxt() {
        txtUI.text = "";
    }

    /* THIS FUNCTION IS FOR TESTING PURPOSE ONLY */

    public void LoseHealth()
    {
        health -= 10;
    }

    /* END OF TESTING FUNCTION */

}
