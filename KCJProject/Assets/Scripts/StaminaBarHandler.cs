using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class StaminaBarHandler : MonoBehaviour
{
    private GameManager code;
    [SerializeField] private GameObject fillArea; // must be de-activated when stamina is at 0 for a short period of time
    private Slider staminaBar;

    // Start is called before the first frame update
    void Start()
    {
        //staminaBar = GetComponent<Slider>();
        //code.Stamina = staminaBar.value;
    }

    // Update is called once per frame
    void Update()
    {
        //if (staminaBar.value == 100)
        //{
        //    staminaBar.gameObject.SetActive(false);  // if stamina is full the bar should be hidden from UI - only appears during usage and regenaration
        //} else if (staminaBar.value < 100)
        //{
        //    staminaBar.gameObject.SetActive(true);
        //}

        //if (code.isRunning() == true)
        //{

        //}
            
    }
}
