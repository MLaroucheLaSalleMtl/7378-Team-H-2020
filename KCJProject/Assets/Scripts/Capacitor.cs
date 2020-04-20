using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capacitor : InteractableHandler
{
    private static GameManager code;
    [Tooltip("Needed Object to fix the light - Player must be holding it")] [SerializeField] private GameObject key;
    [Tooltip("Opening sound")] [SerializeField] private AudioSource powerOn, ouch;
    [SerializeField] GameObject[] lights;
    [SerializeField] GameObject nextPopup, door;
    [SerializeField] [ColorUsageAttribute(true, true)] private Color colorOn,colorOff;
    [SerializeField] private Material m;

    void Awake() {
        m.SetColor("_EmissionColor", colorOff);
    }
    // Start is called before the first frame update
    new void Start() {
        base.Start();
        code = GameManager.instance;
    }

    public override void DoInteraction() {
        base.DoInteraction();
        if (code.Holding.tag == key.tag) {
            Fixed();   
        }
    }

    private void Fixed() {
        powerOn.Play();
        ouch.Play();
        foreach (GameObject l in lights) l.SetActive(true);
        nextPopup.SetActive(true);
        m.SetColor("_EmissionColor", colorOn*0.4f);
        GetComponent<AudioSource>().Play();
        door.GetComponent<OpenDoor>().isLocked = false;
    }
}
