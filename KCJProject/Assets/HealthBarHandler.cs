using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarHandler : MonoBehaviour
{
    [SerializeField] private GameObject fillArea; // this must be de-activated when health reaches 0 at any time
    private Slider healthValue;

    // Start is called before the first frame update
    void Start()
    {
        healthValue = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (healthValue.value == 0)
        {
            fillArea.gameObject.SetActive(false);
        }
    }
}
