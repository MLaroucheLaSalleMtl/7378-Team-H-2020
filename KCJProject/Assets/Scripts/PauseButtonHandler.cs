using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// script added for gamepad support by julien

public class PauseButtonHandler : MonoBehaviour
{
    [SerializeField] private Selectable defaultSelected;

    // Start is called before the first frame update
    void Start()
    {
        defaultSelected.Select();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
