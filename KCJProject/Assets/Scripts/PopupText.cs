using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

[RequireComponent(typeof(SphereCollider))]
public class PopupText : MonoBehaviour
{
    private static TrackerHandler tracker;
    [SerializeField] private GameObject textPanel;
    [SerializeField] private AudioSource alertSound;
    private bool alreadyInteracted = false;

    [Tooltip("Task for the obejctive - Short Sentences")][SerializeField] private string task;
    [Tooltip("Description of objective")] [SerializeField] private string desc;
    [Tooltip("ORder of objective")] [SerializeField] private int order;
    [SerializeField] private Selectable objective;
    
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<SphereCollider>().isTrigger = true;
        textPanel.gameObject.SetActive(false);
        textPanel.GetComponentInChildren<Text>().text = desc;
        tracker = TrackerHandler.instance;
        objective = Instantiate(objective);
        objective.GetComponent<Objective>().Task = task;
        objective.GetComponent<Objective>().Description = desc;
        objective.GetComponent<Objective>().ObjOrder = order;
    }

    // Update is called once per frame
    void Update()
    {
        /* THIS MUST BE CHANGED TO SUPPORT INPUT SYSTEM */
        if (Input.GetKeyDown(KeyCode.Return))
        {
            textPanel.gameObject.SetActive(false);
        }
    }

    protected void OnTriggerEnter(Collider other)
    {        
        if (!alreadyInteracted)
        {
            //Finds if previous objective was seen. if not, ignore
            //if yes, complete that objective and gives new objective
            Objective obj = tracker.Find(order - 1);
            if (obj) {
                textPanel.gameObject.SetActive(true);
                alertSound.Play();
                alreadyInteracted = true;
                tracker.CompletedObjective(obj);
                Invoke("Objective", 4.2f);
            }else if(order == 0) {
                textPanel.gameObject.SetActive(true);
                alertSound.Play();
                alreadyInteracted = true;
                Objective();
            }
        } 
    }

    private void Objective() {
        tracker.NewObjective(objective);
    }
}
