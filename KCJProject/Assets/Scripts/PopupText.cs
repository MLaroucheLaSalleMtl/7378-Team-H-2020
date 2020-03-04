using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

[RequireComponent(typeof(SphereCollider))]
public class PopupText : MonoBehaviour
{
    [SerializeField] private GameObject textPanel;
    [SerializeField] private AudioSource alertSound;
    private bool alreadyInteracted = false;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<SphereCollider>().isTrigger = true;
        textPanel.gameObject.SetActive(false);
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
        if (alreadyInteracted == false)
        { 
            textPanel.gameObject.SetActive(true);
            alertSound.Play();
            alreadyInteracted = true;
        } 
    }
}
