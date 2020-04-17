using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
//This class verifies if certain object was removed from where it was standing
public class RemovedFrom : MonoBehaviour
{
    [Tooltip("Reference an instance of the object to be removed")][SerializeField] private GameObject objectRemoved;
    [Tooltip("Reference of an object to be activated after removal")] [SerializeField] private GameObject aftermath;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<BoxCollider>().isTrigger = true;
        aftermath.SetActive(false);
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject.Equals(objectRemoved)) {
            aftermath.SetActive(true);
            Destroy(gameObject);
        }
    }
}
