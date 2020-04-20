using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Objective : MonoBehaviour
{
    private static TrackerHandler tracker;
    public bool isDone = false;

    [SerializeField] Color completed;
    [Tooltip("Order of objective")][SerializeField] private int objOrder;

    [Header("Debugging only")]
    [SerializeField]private string task, description;
    
    public string Task { get => task; set => task = value; }
    public string Description { get => description; set => description = value; }
    public int ObjOrder { get => objOrder; set => objOrder = value; }

    void Start() {
        tracker = TrackerHandler.instance;
    }

    public void OnSelectObj() {
        tracker.ShowSelObj(Task, Description);
    }
    public void Completed() {
        isDone = true;
        GetComponent<Selectable>().GetComponent<Image>().color = completed;
    }
    public override string ToString() {
        return "Task : " + Task + " Description: " + Description;
    }
}
