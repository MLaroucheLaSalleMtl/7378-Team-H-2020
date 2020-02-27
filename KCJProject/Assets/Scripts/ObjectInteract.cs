using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectInteract : MonoBehaviour
{
    private GameObject raycastedObject;
    private GameManager code; // Reference to Game Manager for later implementation of object utility
    private Camera camera;

    [SerializeField] private int rayLength = 2; // I made it a serialize field in order to easily manipulate it with the editor
    [SerializeField] private LayerMask layerMaskInteract; // I created a layer for interactable objects in the editor
    

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed) {
            RaycastHit hit;
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Ray ray = camera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out hit, rayLength, layerMaskInteract)) 
            {
                raycastedObject = hit.collider.gameObject;
                          
                
                if (raycastedObject.GetComponent<InteractableHandler>())
                    raycastedObject.GetComponent<InteractableHandler>().DoInteraction();
                else
                    Debug.Log("Not an interactable object");

            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        code = GameManager.instance;
        camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
