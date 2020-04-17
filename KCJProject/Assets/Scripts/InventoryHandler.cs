using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryHandler : MonoBehaviour
{
    public static InventoryHandler instance;
    private static GameManager code;

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

    [Header("List of itens prefab")]
    [SerializeField] private GameObject[] items;


    public int InventorySize { get => inventorySize; set => inventorySize = value; }

    //called before Start
    void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        code = GameManager.instance;
        Init();
        UpdateInventory();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Initialize Inventory
    private void Init() {
        foreach (Image img in iconInventory) img.sprite = emptyObj;
        foreach (Selectable btn in btnItens) btn.interactable = false;
    }

    public void Add(GameObject obj) {
        inventory.Add(obj);
        this.iconInventory[inventory.Count - 1].sprite = obj.GetComponent<Grabbable>().Icon;
        this.btnItens[inventory.Count - 1].interactable = true;
        UpdateInventory();
        HoldItem(obj);
    }

    public void Drop(GameObject obj, int selectedItem) {
        inventory.Remove(obj);
        iconInventory[selectedItem].sprite = emptyObj;
        btnItens[selectedItem].interactable = false;
        UpdateInventory();
    }

    public void SelectItem(int selectedItem) {
        if (selectedItem > inventory.Count) return;
        descImg.sprite = inventory[selectedItem].GetComponent<Grabbable>().Icon;
        txtName.text = inventory[selectedItem].GetComponent<Grabbable>().Name;
        DescMenu(true);
    }
    public void HoldItem(GameObject holding) {
        if (inventory.Count <= 0) return;
        foreach (GameObject item in inventory) {
            if (item != holding) item.SetActive(false);
        }
        holding.SetActive(true);
    }
    public void UpdateInventory() {
        if (inventory.Count <= 0) {
            txtName.enabled = false;
            descImg.sprite = emptyObj;
            code.SelectedItem = 0;
        } else {
            txtName.enabled = true;
            int i = 0;
            foreach (GameObject item in inventory) {
                iconInventory[i].sprite = item.GetComponent<Grabbable>().Icon;
                btnItens[i].interactable = true;
                i++;
            }
            for (int z = i; z < inventorySize; z++) {
                iconInventory[z].sprite = emptyObj;
                btnItens[z].interactable = false;
            }
        }
    }
    //Acivates or deactivate description menu
    public void DescMenu(bool flag) {
        if(!flag) descImg.sprite = emptyObj;
        txtName.enabled = flag;
        btnDrop.interactable = flag;
        btnEquip.interactable = flag;
        if (btnEquip.interactable) btnEquip.Select();
    }
    public void DropFromMenu(int selectedItem) {
        Debug.Log(inventory[selectedItem].ToString());
        if (inventory.Count <= 0) return;
        inventory[selectedItem].GetComponent<Grabbable>().OnDrop();
        if (inventory.Count <= 0) DescMenu(false);
    }

    public void AddFromDisk(int[] it) {
        if (it.Length <= 0) return;
        Init();
        inventory.Clear();
        foreach(int i in it) {
            if (i <= items.Length)
                Instantiate(items[i]).GetComponent<Grabbable>().DoInteraction();
        }
    }

    //highlights first inventory item, if present, for gamepad use
    public void HighlightIvn() {
        if (btnItens[0].interactable) btnItens[0].Select();
    }

}
