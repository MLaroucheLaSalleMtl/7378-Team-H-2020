using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// THIS SCRIPT HAS BEEN TAKEN FROM AN IN-CLASS ACTIVITY GIVEN BY MARC-ANDRE LAROUCHE (FALL 2019)

public class WaterCollider : MonoBehaviour
{
    private Vector3 prevPos = new Vector3();    

    // Start is called before the first frame update
    void Start()
    {
        prevPos = transform.position;
    }

    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 1000))
        {
            if (hit.collider.tag == "Water")
            {
                transform.position = new Vector3(prevPos.x, transform.position.y, prevPos.z);
            }
            else
            {
                prevPos = transform.position;
            }
        }
    }
}
