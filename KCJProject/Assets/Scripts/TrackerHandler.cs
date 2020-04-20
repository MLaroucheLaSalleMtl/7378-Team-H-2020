/// Handles the objective tracker
///by Caue
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class TrackerHandler : MonoBehaviour
{
    public static TrackerHandler instance;

    [Header("UI")]
    [Tooltip("Task for current objective")] [SerializeField] private Text currTaskTxt;
    [Tooltip("Description for current objective")] [SerializeField] private Text currDescTxt;
    [Tooltip("Area for past objectives")] [SerializeField] private GameObject scrollArea;
    [Tooltip("Panel for new or completed objectives")] [SerializeField]private GameObject panelObj;
    [Tooltip("Color for objective completed")][SerializeField] private Color colorComp;
    [Tooltip("Color for new objective")] [SerializeField] private Color colorNormal;
    private Text objTextUI, taskTextUI;
    [Tooltip("Button Prefab")] [SerializeField] private Selectable btn;
    private float btnObjH; //height of prefab button - used for screen positions calculations
    [Tooltip("Time that a new objective is shown on screen")] [SerializeField] private float timeScreen = 2f;

    [Header("Panel management - change it to GameManager after tests")]
    [Tooltip("Panel for UI should be attached here")] [SerializeField] private GameObject UI;
    [Tooltip("Panel Inventory should be attached here")] [SerializeField] private GameObject inventoryPanel;
    [Tooltip("Panel ObjectiveTracker should be attached here")] [SerializeField] private GameObject trackerPanel;

    //handler attributes
    private bool isTracker = false; //if the tracker screen is beeing showed
    private static GameManager code;

    private List<Selectable> objectives; // keeps past objectives

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
        objectives = new List<Selectable>();
        btnObjH = btn.GetComponent<RectTransform>().sizeDelta.y;

        taskTextUI = panelObj.GetComponentsInChildren<Text>()[0];
        objTextUI = panelObj.GetComponentsInChildren<Text>()[1];

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //Resizes the scroll area to fit the exact amount of buttons - should be called everytime, before an objective is added
    private void UpdateScrollArea() {
        if (objectives.Count > 6)
            scrollArea.GetComponent<RectTransform>().sizeDelta = new Vector2(scrollArea.GetComponent<RectTransform>().sizeDelta.x, objectives.Count * btnObjH);
    }
    //lists all objectives
    public void ListObjectives() {
        UpdateScrollArea();        
        //sets the point in the screen for first ojective
        float j = (objectives.Count * -btnObjH) + btnObjH;
        foreach (Selectable o in objectives) {
            o.GetComponentInChildren<Text>().text = o.GetComponent<Objective>().Task;
            o.gameObject.transform.SetParent(scrollArea.transform);
            RectTransform rect = o.GetComponent<RectTransform>();

            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = new Vector2(1f, 1f);
            rect.pivot = new Vector2(0.5f, 1f);
            rect.localScale = new Vector3(1f, 1f, 1f);

            o.transform.localPosition = new Vector3(o.transform.localPosition.x, 330 + j, o.transform.localPosition.z);

            ///Adapted from https://answers.unity.com/questions/888257/access-left-right-top-and-bottom-of-recttransform.html by Caue
            rect.offsetMin = new Vector2(0, rect.offsetMin.y); //set left, bottom
            rect.offsetMax = new Vector2(-0, rect.offsetMax.y); //set Right
            ///  
            Debug.Log("J " + j + " " +rect.offsetMin + "L R" + rect.offsetMax);
                                                               

            //update next place for next objective
            j += btnObjH;
        }
    }

    public void OnTracker(InputAction.CallbackContext context) {
        if (code.pause || code.isInventory) return;
        if (context.performed) {
            if (!isTracker) {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                inventoryPanel.SetActive(false);
                UI.SetActive(false);
                trackerPanel.SetActive(true);
                try {
                    scrollArea.GetComponentsInChildren<Selectable>()[scrollArea.GetComponentsInChildren<Selectable>().Length-1].Select();
                }
                catch {
                    Debug.Log("No Objectives");
                }

                code.ActivateController(false);
            } else {
                inventoryPanel.SetActive(false);
                UI.SetActive(true);
                trackerPanel.SetActive(false);

                code.ActivateController(true);
            }
            code.isTracker = isTracker = !isTracker;
        }
    }

    public void NewObjective(Selectable newObj) {
        newObj.interactable = true;
        objectives.Add(newObj);
        UpdateTracker();
        ShowUI(newObj.GetComponent<Objective>());
        ListObjectives();
    }

    public void CompletedObjective(Selectable obj) {
        obj.GetComponent<Objective>().Completed();
        ShowUI(obj.GetComponent<Objective>()); 
    }
    public void CompletedObjective(Objective obj) {
        obj.Completed();
        ShowUI(obj);
    }
    //updates the tracker menu with the latest objective
    private void UpdateTracker() {
        Objective o = objectives[objectives.Count - 1].GetComponent<Objective>();
        currDescTxt.text = o.Description;
        currTaskTxt.text = o.Task;
    }
    public void ShowSelObj(string task, string desc) {
        this.currTaskTxt.text = task;
        this.currDescTxt.text = desc;
    }
    private void ShowUI(Objective obj) {
        if (!obj.isDone) {
            objTextUI.text = "New Objective: ";
            panelObj.GetComponent<Image>().color = colorNormal;
            Invoke("DeactivatePanel", this.timeScreen);
        } else {
            objTextUI.text = "Objective Completed: ";
            Invoke("ChangeColor", 0.2f);
            Invoke("DeactivatePanel", 1f);
        }
        panelObj.SetActive(true);
        taskTextUI.text = obj.Task;
    }

    private void DeactivatePanel() {
        panelObj.SetActive(false);
    }

    private void ChangeColor() {
        panelObj.GetComponent<Image>().color = colorComp;
    }

    public Objective Find(int order) {
        //find if objective of order 'order' have been gained 
        foreach (Selectable s in objectives) 
            if (s.GetComponent<Objective>().ObjOrder == order) return s.GetComponent<Objective>();
        
        return null;
    }
}
