using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectInteract : MonoBehaviour
{
    private GameObject raycastedObject;
    private GameManager code; // Reference to Game Manager for later implementation of object utility
    private bool interact = false;

    [SerializeField] private int rayLength = 2; // I made it a serialize field in order to easily manipulate it with the editor
    [SerializeField] private LayerMask layerMaskInteract; // I created a layer for interactable objects in the editor
    

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
            interact = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        code = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Vector3 forward = transform.TransformDirection(Vector3.forward);

        if (Physics.Raycast(transform.position, forward, out hit, rayLength, layerMaskInteract)) //The layermask has no use here, for now
        {
            raycastedObject = hit.collider.gameObject;

            if (interact == true) 
            {
                Debug.Log("You have interacted with an object");
                Destroy(raycastedObject);
                interact = false;
            }
        }
    }
}
