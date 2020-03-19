using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageGiver : MonoBehaviour
{
    public static GameManager code;
    [Tooltip("Amount of damage this object can give")][SerializeField] private int damage;
    // Start is called before the first frame update
    void Start()
    {
        code = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.transform.tag == "Player") {
            code.LoseHealth(damage);
        }
    }
}
