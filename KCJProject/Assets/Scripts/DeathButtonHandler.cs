using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathButtonHandler : MonoBehaviour
{
    [Header("Death Canvas")] private GameObject deathCanvas;
    [Header("Death Panel Default Button")][SerializeField] private Selectable defaultSelected;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (deathCanvas.gameObject.activeSelf == true)
        {
            defaultSelected.Select();
        } 
    }
}
