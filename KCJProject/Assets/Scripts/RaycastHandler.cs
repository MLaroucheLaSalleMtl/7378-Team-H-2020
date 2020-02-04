using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RaycastHandler : MonoBehaviour
{
    private static GameManager code;
    private Camera camera;
    [Tooltip("Maximum range of the gun - Should change in each gun")]
    [SerializeField] private float maxDistance = 50f;

    // Start is called before the first frame update
    void Start() {
        code = GameManager.instance;
        camera = GetComponent<Camera>();
    }
    /// <summary>
    /// Raycast code adpated from https://docs.unity3d.com/ScriptReference/Physics.Raycast.html
    /// </summary>
    public void OnClick(InputAction.CallbackContext context) {
        if (context.performed) {
            ///
            ///The layer masks are for future use with weapons camera
            ///
            // Bit shift the index of the layer (8) to get a bit mask
            int layerMask = 1 << 10;
            // This would cast rays only against colliders in layer 8.
            // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
            layerMask = ~layerMask;
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out hit, maxDistance/*, layerMask*/)) {
                Transform hitObj = hit.transform;
                Debug.Log("You hit " + hitObj.name.ToString());
            }
        }
    }   
}
