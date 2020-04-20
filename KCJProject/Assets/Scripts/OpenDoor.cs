using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : InteractableHandler
{
    protected static GameManager code;
    [Tooltip("Doors that make up the big door")][SerializeField] private GameObject door1, door2;
    [Tooltip("Needed Object to open the door - Player must be holding it")][SerializeField] private GameObject key;
    [Tooltip("Opening sound")] [SerializeField] private AudioSource audio;
    public bool isLocked = false;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        code = GameManager.instance;
        audio = GetComponent<AudioSource>();
    }

    public override void DoInteraction() {
        base.DoInteraction();
        if (isLocked) return;
        if(key == null) OpenDoors();
        if (code.Holding == key) {
            OpenDoors();
        }
    }

    private void OpenDoors() {
        Debug.Log("OpenDoors");
        door1.GetComponent<Animation>().Play();
        try {
            door2.GetComponent<Animation>().Play();
        }
        catch {
            Debug.Log("One door only");
        }
        GetComponent<BoxCollider>().enabled = false;
        base.textMesh.gameObject.SetActive(false);
        base.textMesh.enabled = false;
        audio.Play();
    }
}
