using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastDoorOpen : InteractableHandler
{
    protected static GameManager code;
    [Tooltip("Doors that make up the big door")] [SerializeField] private GameObject door;
    [Tooltip("Needed Object to open the door - Player must be holding it")] [SerializeField] private GameObject key;
    [Tooltip("Opening sound")] [SerializeField] private AudioSource audio;
    public bool isLocked = false;

    [SerializeField] private Animator anim;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        code = GameManager.instance;
        audio = GetComponent<AudioSource>();
    }

    public override void DoInteraction()
    {
        base.DoInteraction();
        if (isLocked) return;
        if (key == null) OpenDoors();
        if (code.Holding == key)
        {
            OpenDoors();
        }
    }

    private void OpenDoors()
    {
        Debug.Log("OpenDoors");
        anim.SetBool("opened", true);

        GetComponent<BoxCollider>().enabled = false;
        base.textMesh.gameObject.SetActive(false);
        base.textMesh.enabled = false;
        audio.Play();
    }
}
